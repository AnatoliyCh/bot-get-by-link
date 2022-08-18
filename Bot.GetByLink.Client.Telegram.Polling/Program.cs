using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model.Commands;
using Bot.GetByLink.Client.Telegram.Common.Model.Logging;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Client.Telegram.Polling;
using Bot.GetByLink.Common.Infrastructure.Configuration;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Reddit;
using Bot.GetByLink.Proxy.Vk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
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
    var (configurationRoot, botConfiguration) = GetConfiguration();
    var client = GetTelegramClient(botConfiguration);

    IServiceCollection services = new ServiceCollection();
    services.AddSingleton(client);
    services.AddSingleton(botConfiguration);

    services.AddScoped<ICommand<CommandName>, SendMessageCommand>();
    services.AddScoped<ICommandInvoker<CommandName>, CommandInvoker>();
    services.AddScoped<ClientPolling>();

    AddProxyServices(services, botConfiguration.Proxy);
    AddLogging(services, configurationRoot);
    AddRegexWrapper(services);
    return services.BuildServiceProvider();
}

static (IConfigurationRoot, IBotConfiguration) GetConfiguration()
{
    var botConfiguration = new BotConfiguration();
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddEnvironmentVariables().Build();

    // get project name
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    configuration["ProjectName"] = projectName;

    configuration.Bind(botConfiguration);

    return (configuration, botConfiguration);
}

static ITelegramBotClient GetTelegramClient(IBotConfiguration configuration)
{
    var token = configuration?.Clients?.Telegram?.Token;
    if (!string.IsNullOrWhiteSpace(token)) return new TelegramBotClient(token);
    throw new ArgumentException("Token");
}

static void AddLogging(IServiceCollection services, IConfigurationRoot configuration)
{
    LogLevel minLevel = LogLevel.Information;
    string defaultLevel = configuration["Logging:LogLevel:Default"] ?? string.Empty;
    if (Enum.IsDefined(typeof(LogLevel), defaultLevel))
        minLevel = Enum.Parse<LogLevel>(defaultLevel, true);

    services.AddLogging(builder =>
    {
        builder.SetMinimumLevel(minLevel);
        builder.AddProvider<TelegramLoggerProvider<CommandName>>();
        builder.AddConsole();
    });
}

static void AddRegexWrapper(IServiceCollection services)
{
    services.AddScoped<IRegexWrapper, UrlRegexWrapper>();
    services.AddScoped<IRegexWrapper, CommandRegexWrapper>();
}

static void AddProxyServices(IServiceCollection services, IProxyConfiguration proxyConfiguration)
{
    if (proxyConfiguration.Reddit.Run) services.AddScoped<IProxyService, ProxyReddit>();
    if (proxyConfiguration.Vk.Run) services.AddScoped<IProxyService, ProxyVK>();
}