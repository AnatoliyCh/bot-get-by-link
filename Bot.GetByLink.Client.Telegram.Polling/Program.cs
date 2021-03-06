using System.Reflection;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model;
using Bot.GetByLink.Client.Telegram.Common.Model.Logging;
using Bot.GetByLink.Client.Telegram.Polling;
using Bot.GetByLink.Client.Telegram.Polling.Commands;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model.Configuration;
using Bot.GetByLink.Proxy.Reddit;
using Bot.GetByLink.Proxy.Vk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

var serviceProvider = ConfigureServices();

var configuration = serviceProvider.GetService<IBotConfiguration>()!;

var logger = serviceProvider.GetService<ILogger<Program>>()!;

var client = serviceProvider.GetService<ClientPolling>()!;

var launched = await client.StartAsync();

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
            await client.StopAsync();
            break;
        case "start polling":
            await client.StartAsync();
            logger.LogInformation($"{configuration.ProjectName}: starting polling...");
            break;
        case "stop polling":
            await client.StopAsync();
            logger.LogInformation($"{configuration.ProjectName}: stoping polling...");
            break;
    }
}

static IServiceProvider ConfigureServices()
{
    var configuration = GetConfiguration();
    var client = GetTelegramClient(configuration);

    IServiceCollection services = new ServiceCollection();
    services.AddSingleton(client);
    services.AddSingleton(configuration);
    services.AddScoped<IProxyService, ProxyReddit>();
    services.AddScoped<IProxyService, ProxyVK>();
    services.AddScoped<ICommand<CommandName>, SendMessageCommand>();
    services.AddScoped<ICommandInvoker<CommandName>, CommandInvoker>();
    services.AddScoped<ClientPolling>();

    AddLogging(services);
    AddRegexWrapper(services);
    return services.BuildServiceProvider();
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
    throw new ArgumentException("Token");
}

static void AddLogging(IServiceCollection services)
{
    services.AddLogging(builder =>
    {
        builder.AddProvider<TelegramLoggerProvider<CommandName>>();
        builder.AddConsole();
    });
}

static void AddRegexWrapper(IServiceCollection services)
{
    services.AddScoped<IRegexWrapper, UrlRegexWrapper>();
    services.AddScoped<IRegexWrapper, CommandRegexWrapper>();
}