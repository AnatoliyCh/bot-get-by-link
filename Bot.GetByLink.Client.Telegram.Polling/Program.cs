using System.Reflection;
using Bot.GetByLink.Client.Telegram.Polling;
using Bot.GetByLink.Client.Telegram.Polling.Commands;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

var serviceProvider = ConfigureServices();

var configuration = serviceProvider.GetService<IConfiguration>()!;

var client = serviceProvider.GetService<ClientPolling>()!;

var projectName = configuration["ProjectName"] ?? string.Empty;

var startMessage = $"{projectName}: start";

var launched = await client.Start();

if (launched)
{
    Console.WriteLine(startMessage);
    await client.SendTextMessageToLogChatAsync(startMessage);
}

var looping = true;

while (looping)
{
    Console.Write("Command: ");
    var command = Console.ReadLine();
    switch (command)
    {
        case "exit polling":
            looping = false;
            await client.Stop();
            break;
        case "start polling":
            Console.WriteLine($"{projectName}: starting polling...");
            await client.Start();
            break;
        case "stop polling":
            Console.WriteLine($"{projectName}: stoping polling...");
            await client.Stop();
            break;
    }
}

static IConfiguration GetConfiguration()
{
    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables().Build();

    // get project name
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    configuration["ProjectName"] = projectName;
    return configuration;
}

static ITelegramBotClient GetTelegramClient(IConfiguration configuration)
{
    var tokenClientTelegram = configuration.GetValue<string>("Telegram:TokenClient");
    if (!string.IsNullOrWhiteSpace(tokenClientTelegram)) return new TelegramBotClient(tokenClientTelegram);
    throw new ArgumentException("Telegram:TokenClient");
}

static IServiceProvider ConfigureServices()
{
    var configuration = GetConfiguration();
    var client = GetTelegramClient(configuration);

    IServiceCollection services = new ServiceCollection();
    services.AddSingleton(configuration);
    services.AddSingleton(client);
    services.AddScoped<ICommandInvoker<CommandName>, CommandInvoker>();
    services.AddScoped<ClientPolling>();
    return services.BuildServiceProvider();
}