using Bot.GetByLink.Client.Telegram.Common.Abstractions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling;

/// <summary>
///     Telegram client.
///     Connection Type: polling.
/// </summary>
internal sealed class ClientPolling : TelegramClient
{
    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientPolling" /> class.
    /// </summary>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="config">Bot configuration.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    public ClientPolling(
        ILogger<ClientPolling> logger,
        IBotConfiguration config,
        ITelegramBotClient client,
        ICommandInvoker<CommandName> invoker,
        IEnumerable<IRegexWrapper> regexWrappers)
        : base(logger, config, client, invoker, regexWrappers)
    {
    }

    /// <summary>
    ///     Client launch or reset.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}" />Representing the result of the asynchronous operation.</returns>
    public override async Task<bool> StartAsync()
    {
        if (State == Status.On || cts is not null) await StopAsync();
        cts = new CancellationTokenSource();
        var validToken = await Client.TestApiAsync(cts.Token);
        if (!validToken)
        {
            Dispose();
            Logger.LogCritical($"{ProjectName}: token is not valid");
            return false;
        }

        Client.StartReceiving(
            (ITelegramBotClient botClient, Update update, CancellationToken ct) => HandleUpdateAsync(update),
            HandleErrorAsync,
            ReceiverOptions,
            cts.Token);
        State = Status.On;
        return true;
    }

    /// <summary>
    ///     Releases all resources used by the current instance TelegramClient.
    /// </summary>
    /// <param name="disposing">Whether to free resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!disposing || cts is null) return;
        cts.Cancel();
        cts.Dispose();
        cts = null;
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        await Task.Run(() => Logger.LogError(exception, "HandleErrorAsync"));
    }
}