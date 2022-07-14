using System.Reflection;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model.Logging;
using Bot.GetByLink.Client.Telegram.Polling;
using Bot.GetByLink.Client.Telegram.Polling.Commands;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model.Configuration;
using Bot.GetByLink.Proxy.Reddit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

var serviceProvider = ConfigureServices();

var configuration = serviceProvider.GetService<IBotConfiguration>()!;

var logger = serviceProvider.GetService<ILogger<Program>>()!;

var client = serviceProvider.GetService<ClientPolling>()!;

var launched = await client.Start();

var looping = true;

if (launched) logger.LogInformation($"{configuration.ProjectName}: starting polling...");
else return;

while (looping)
{
    Console.WriteLine("Command: ");
    var command = Console.ReadLine();
    switch (command)
    {
        case "exit polling":
            looping = false;
            await client.Stop();
            break;
        case "start polling":
            await client.Start();
            logger.LogInformation($"{configuration.ProjectName}: starting polling...");
            break;
        case "stop polling":
            await client.Stop();
            logger.LogInformation($"{configuration.ProjectName}: stoping polling...");
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

static void AddLogging(IServiceCollection services)
{
    services.AddLogging(builder =>
    {
        builder.AddProvider<TelegramLoggerProvider<CommandName>>();
        builder.AddConsole();
    });
}

static ITelegramBotClient GetTelegramClient(IBotConfiguration configuration)
{
    var token = configuration?.Clients?.Telegram?.Token;
    if (!string.IsNullOrWhiteSpace(token)) return new TelegramBotClient(token);
    throw new ArgumentException("Token");
}

static IServiceProvider ConfigureServices()
{
    var configuration = GetConfiguration();
    var client = GetTelegramClient(configuration);

    IServiceCollection services = new ServiceCollection();
    services.AddSingleton(client);
    services.AddSingleton(configuration);
    services.AddScoped<IProxyService, ProxyReddit>();
    services.AddScoped<ICommand<CommandName>, SendMessageCommand>();
    services.AddScoped<ICommandInvoker<CommandName>, CommandInvoker>();
    services.AddScoped<ClientPolling>();

    AddLogging(services);
    return services.BuildServiceProvider();
}