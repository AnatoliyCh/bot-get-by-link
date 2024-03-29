using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model.Commands;
using Bot.GetByLink.Client.Telegram.Common.Model.Logging;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Client.Telegram.WebHook;
using Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;
using Bot.GetByLink.Client.Telegram.WebHook.Model.Configuration;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Reddit;
using Bot.GetByLink.Proxy.Vk;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

var client = app.Services.GetService<ClientWebHook>()!;

var configuration = app.Services.GetService<IBotWebHookConfiguration>()!;

app.MapPost($"bot{configuration.Clients.Telegram.Token}", async (object? data) =>
{
    try
    {
        var update = JsonConvert.DeserializeObject<Update>(data?.ToString() ?? string.Empty);
        if (update is null)
        {
            app.Logger.LogWarning(string.Format("Unknown message: {0}", data?.ToString() ?? "-"));
            return;
        }

        await client.HandleUpdateAsync(update);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex.ToString());
    }
});

if (!app.Environment.IsDevelopment()) app.Urls.Add("http://*:" + configuration.Server.Port);

app.Run();

await client.StartAsync();

static void ConfigureServices(WebApplicationBuilder builder)
{
    var configuration = GetConfiguration(builder);
    var client = GetTelegramClient(configuration);
    builder.Services.AddSingleton(client);
    builder.Services.AddSingleton((IBotConfiguration)configuration);
    builder.Services.AddSingleton(configuration);

    builder.Services.AddTransient<ICommand<CommandName>, SendMessageCommand>();
    builder.Services.AddTransient<ICommandInvoker<CommandName>, CommandInvoker>();
    builder.Services.AddTransient<ClientWebHook>();

    AddProxyServices(builder.Services, configuration.Proxy);
    AddLogging(builder.Services);
    AddRegexWrapper(builder.Services);
}

static IBotWebHookConfiguration GetConfiguration(WebApplicationBuilder builder)
{
    var botConfiguration = new BotWebHookConfiguration();
    var sharedConfiguration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables().Build();

    // get project name
    builder.Configuration["ProjectName"] = builder.Environment.ApplicationName;

    // get server url and port
    builder.Configuration["Server:Url"] = sharedConfiguration["URL"];
    builder.Configuration["Server:Port"] = sharedConfiguration["PORT"];

    builder.Configuration.AddConfiguration(sharedConfiguration);
    builder.Configuration.Bind(botConfiguration);

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