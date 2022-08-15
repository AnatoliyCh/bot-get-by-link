using Bot.GetByLink.Client.Telegram.Common.Abstractions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.WebHook;

/// <summary>
///     Telegram client.
///     Connection Type: WebHook.
/// </summary>
internal sealed class ClientWebHook : TelegramClient
{
    private readonly string url;
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientWebHook"/> class.
    /// </summary>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="config">Bot configuration.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    public ClientWebHook(
        ILogger<ClientWebHook> logger,
        IBotWebHookConfiguration config,
        ITelegramBotClient client,
        ICommandInvoker<CommandName> invoker,
        IEnumerable<IRegexWrapper> regexWrappers)
        : base(logger, config, client, invoker, regexWrappers)
    {
        url = $"{config.Server.Url}/bot{config.Clients.Telegram.Token}";
    }

    /// <summary>
    ///     Client launch or reset.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}" />Representing the result of the asynchronous operation.</returns>
    public override async Task<bool> StartAsync()
    {
        if (State == Status.On) await StopAsync();
        var cts = new CancellationTokenSource();
        var validToken = await Client.TestApiAsync(cts.Token);
        if (!validToken)
        {
            Dispose();
            Logger.LogCritical($"{ProjectName}: token is not valid");
            return false;
        }

        await Client.SetWebhookAsync(url, allowedUpdates: ReceiverOptions.AllowedUpdates);
        State = Status.On;
        return true;
    }
}
