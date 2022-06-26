using System.Reflection;
using Bot.GetByLink.Client.Telegram.Polling;
using Bot.GetByLink.Client.Telegram.Polling.Commands;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Configuration;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Proxy.Reddit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

var serviceProvider = ConfigureServices();

var configuration = serviceProvider.GetService<IBotConfiguration>()!;

var client = serviceProvider.GetService<ClientPolling>()!;

var launched = await client.Start();

if (launched)
{
    var startMessage = $"{configuration.ProjectName}: start";
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
            Console.WriteLine($"{configuration.ProjectName}: starting polling...");
            await client.Start();
            break;
        case "stop polling":
            Console.WriteLine($"{configuration.ProjectName}: stoping polling...");
            await client.Stop();
            break;
    }
}

static IBotConfiguration GetConfiguration()
{
    var botConfiguration = new BotConfiguration();
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables().Build();

    // get project name
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    configuration["ProjectName"] = projectName;

    configuration.Bind(botConfiguration);
    return botConfiguration;
}

static ITelegramBotClient GetTelegramClient(IBotConfiguration configuration)
{
    var token = configuration?.Clients?.Telegram?.Token;
    if (!string.IsNullOrWhiteSpace(token)) return new TelegramBotClient(token);
    throw new ArgumentException("Telegram:Token");
}

static IServiceProvider ConfigureServices()
{
    var configuration = GetConfiguration();
    var client = GetTelegramClient(configuration);

    IServiceCollection services = new ServiceCollection();
    services.AddSingleton(configuration);
    services.AddSingleton(client);
    services.AddScoped<IProxyService, ProxyReddit>();
    services.AddScoped<ICommandInvoker<CommandName>, CommandInvoker>();
    services.AddScoped<ClientPolling>();

    return services.BuildServiceProvider();
}