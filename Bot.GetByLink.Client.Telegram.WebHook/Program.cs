using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model.Commands;
using Bot.GetByLink.Client.Telegram.Common.Model.Logging;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Client.Telegram.WebHook;
using Bot.GetByLink.Client.Telegram.WebHook.Model.Configuration;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Reddit;
using Bot.GetByLink.Proxy.Vk;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

app.UseHttpsRedirection();

var client = app.Services.GetService<ClientWebHook>()!;

var configuration = app.Services.GetService<IBotConfiguration>()!;

app.MapPost($"/{configuration.Clients.Telegram.Token}", async ([FromBody] Update update) => await client.HandleUpdateAsync(update));

app.Urls.Add("https://*:443");
app.Urls.Add("https://*:80");
app.Urls.Add("https://*:88");
app.Urls.Add("https://*:8443");

app.Run();

await client.StartAsync();

static void ConfigureServices(WebApplicationBuilder builder)
{
    var configuration = GetConfiguration(builder.Configuration);
    var client = GetTelegramClient(configuration);

    builder.Services.AddSingleton(client);
    builder.Services.AddSingleton(configuration);

    builder.Services.AddTransient<ICommand<CommandName>, SendMessageCommand>();
    builder.Services.AddTransient<ICommandInvoker<CommandName>, CommandInvoker>();
    builder.Services.AddTransient<ClientWebHook>();

    AddProxyServices(builder.Services, configuration.Proxy);
    AddLogging(builder.Services);
    AddRegexWrapper(builder.Services);
}

static IBotConfiguration GetConfiguration(ConfigurationManager configuration)
{
    var botConfiguration = new BotConfiguration();
    var sharedConfiguration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables().Build();

    // get project name
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    configuration["ProjectName"] = projectName;

    configuration.AddConfiguration(sharedConfiguration);

    var hostingHeroku = new HostingHeroku
    {
        Url = sharedConfiguration["HOST"] ?? string.Empty,
        Port = sharedConfiguration["PORT"] ?? string.Empty,
    };

    configuration.Bind(botConfiguration);

    botConfiguration = new BotConfiguration
    {
        ProjectName = botConfiguration.ProjectName,
        Clients = botConfiguration.Clients,
        Proxy = botConfiguration.Proxy,
        Hosting = hostingHeroku,
    };

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
    services.AddTransient<IRegexWrapper, UrlRegexWrapper>();
    services.AddTransient<IRegexWrapper, CommandRegexWrapper>();
}

static void AddProxyServices(IServiceCollection services, IProxyConfiguration proxyConfiguration)
{
    if (proxyConfiguration.Reddit.Run) services.AddTransient<IProxyService, ProxyReddit>();
    if (proxyConfiguration.Vk.Run) services.AddTransient<IProxyService, ProxyVK>();
}