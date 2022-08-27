using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Returns help info the current chat.
/// </summary>
public sealed class HelpCommand : AsyncCommand<CommandName>
{
    private readonly IAsyncCommand<CommandName> sendMessageCommand;
    private readonly int delaySendingMediaGroupSeconds;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HelpCommand" /> class.
    /// </summary>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    /// <param name="delaySendingMediaGroupMilliseconds">Interval in milliseconds between sent messages</param>
    /// <exception cref="ArgumentNullException">Any argument is null.</exception>
    public HelpCommand(IAsyncCommand<CommandName> sendMessageCommand, int delaySendingMediaGroupMilliseconds)
        : base(CommandName.Help)
    {
        this.sendMessageCommand = sendMessageCommand ?? throw new ArgumentNullException(nameof(sendMessageCommand));
        delaySendingMediaGroupSeconds = delaySendingMediaGroupMilliseconds > 0 ? delaySendingMediaGroupMilliseconds / 1000 : 0;
    }

    /// <summary>
    ///    Send help info.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not Update update) return;

        var chatId = update.Message?.Chat.Id;
        if (chatId is null) return;

        var message = new Message(chatId, new[] { string.Format(ResourceRepository.GetClientResource(ClientResource.HelpCommand), delaySendingMediaGroupSeconds) },
            ParseMode: ParseMode.Markdown);
        await sendMessageCommand.ExecuteAsync(message);
    }
}