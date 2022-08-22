﻿using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Abstractions.Command;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Sends a message to the client.
/// </summary>
public sealed class SendMessageCommand : AsyncCommand<CommandName>, IDisposable
{
    private readonly ITelegramBotClient client;
    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendMessageCommand" /> class.
    /// </summary>
    /// <param name="client">Telegram Client.</param>
    public SendMessageCommand(ITelegramBotClient client)
        : base(CommandName.SendMessage)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        cts = new CancellationTokenSource();
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
            foreach (var artifact in message.Artifacts)
            {
                await client.SendMediaGroupAsync(message.ChatId, artifact, cancellationToken: cts.Token);
                await Task.Delay(1500); //TODO: в конфиг
            }
        }

        foreach (var text in message.Text.Where(text => !string.IsNullOrWhiteSpace(text)))
            await client.SendTextMessageAsync(message.ChatId, text, cancellationToken: cts.Token);
    }
}