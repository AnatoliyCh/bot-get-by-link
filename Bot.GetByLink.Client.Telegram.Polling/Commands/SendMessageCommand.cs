using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Client.Telegram.Common.Model;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotTypes = Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Sends a message to the client.
/// </summary>
internal sealed class SendMessageCommand : AsyncCommand<CommandName>, IDisposable
{
    private readonly ITelegramBotClient client;
    private readonly TelegramBotTypes.ChatId? logChatId;
    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendMessageCommand" /> class.
    /// </summary>
    /// <param name="client">Telegram Client.</param>
    /// <param name="logChatId">Chat for logging issues (database not used).</param>
    public SendMessageCommand(ITelegramBotClient client, string? logChatId = null)
        : base(CommandName.SendMessage)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logChatId = !string.IsNullOrWhiteSpace(logChatId) ? new TelegramBotTypes.ChatId(logChatId) : null;
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

    /// <summary>
    ///     Logging (database not used).
    /// </summary>
    /// <param name="text">message.</param>
    /// <returns>Empty Task.</returns>
    public async Task TrySendLogAsync(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        try
        {
            Console.WriteLine(text);
            if (logChatId is not null) await SendMessageAsync(new Message(logChatId, new List<string> { text }));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private async Task SendMessageAsync(IMessageContext message)
    {
        if (cts is null || cts.IsCancellationRequested) cts = new CancellationTokenSource();

        // text
        foreach (var text in message.Text.Where(text => !string.IsNullOrWhiteSpace(text)))
            await client.SendTextMessageAsync(message.ChatId, text, ParseMode.MarkdownV2, cancellationToken: cts.Token);

        // TODO: artifacts
    }
}