using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Returns help info the current chat.
/// </summary>
public sealed class HelpCommand : AsyncCommand<CommandName>
{
    private readonly ITelegramBotClient client;
    private readonly IAsyncCommand<CommandName> sendMessageCommand;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HelpCommand" /> class.
    /// </summary>
    /// <param name="client"> Telegram client.</param>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    public HelpCommand(ITelegramBotClient client, IAsyncCommand<CommandName> sendMessageCommand)
        : base(CommandName.Help)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.sendMessageCommand = sendMessageCommand ?? throw new ArgumentNullException(nameof(sendMessageCommand));
    }

    /// <summary>
    ///     Collect and send chat information.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not Update update) return;
        var chatId = update.Message?.Chat.Id;
        if (chatId is null) return;

        var cts = new CancellationTokenSource();

        var message = new Message(chatId, new[] { ResourceRepository.GetClientResource("HelpCommand") },
            ParseMode: ParseMode.Markdown);
        await sendMessageCommand.ExecuteAsync(message);

        cts.Cancel();
    }
}