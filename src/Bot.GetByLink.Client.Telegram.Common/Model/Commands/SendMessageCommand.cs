﻿using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Sends a message to the client.
/// </summary>
public sealed class SendMessageCommand : AsyncCommand<CommandName>, IDisposable
{
    private readonly ITelegramBotClient client;
    private readonly int delaySendingMediaGroupMilliseconds;
    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendMessageCommand" /> class.
    /// </summary>
    /// <param name="client">Telegram Client.</param>
    /// <param name="config">Bot configuration.</param>
    public SendMessageCommand(ITelegramBotClient client, IBotConfiguration config)
        : base(CommandName.SendMessage)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));

        cts = new CancellationTokenSource();
        delaySendingMediaGroupMilliseconds = config.Clients.Telegram.DelaySendingMediaGroupMilliseconds;
    }

    /// <summary>
    ///     Token Cancellation.
    /// </summary>
    public void Dispose()
    {
        if (cts is null) return;
        cts.Cancel();
        cts.Dispose();
        cts = null;
    }

    /// <summary>
    ///     Send a message.
    /// </summary>
    /// <param name="ctx">Message context.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not IMessageContext message) return;
        await SendMessageAsync(message);
    }

    private async Task SendMessageAsync(IMessageContext message)
    {
        if (cts is null || cts.IsCancellationRequested) cts = new CancellationTokenSource();

        if (message.Artifacts?.Any() ?? false)
        {
            var isDelay = message.Artifacts.Count() > 1;
            foreach (var artifact in message.Artifacts)
            {
                await client.SendMediaGroupAsync(message.ChatId, artifact, cancellationToken: cts.Token);
                if (isDelay) await Task.Delay(delaySendingMediaGroupMilliseconds);
            }
        }

        foreach (var text in message.Text.Where(text => !string.IsNullOrWhiteSpace(text)))
            await client.SendTextMessageAsync(message.ChatId, text, cancellationToken: cts.Token,
                parseMode: message.ParseMode);
    }
}