using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Interfaces.Command;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Returns information about the current chat and the sender.
/// </summary>
public sealed class ChatInfoCommand : AsyncCommand<CommandName>
{
    private readonly ITelegramBotClient client;
    private readonly IAsyncCommand<CommandName> sendMessageCommand;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatInfoCommand" /> class.
    /// </summary>
    /// <param name="client"> Telegram client.</param>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    public ChatInfoCommand(ITelegramBotClient client, IAsyncCommand<CommandName> sendMessageCommand)
        : base(CommandName.ChatInfo)
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
        var chat = await client.GetChatAsync(chatId, cts.Token);

        var info = GetInfoByUpdate(chat, update);
        var message = new Message(chatId, new List<string> { info });
        await sendMessageCommand.ExecuteAsync(message);

        cts.Cancel();
    }

    private static string GetInfoByUpdate(Chat chat, Update update)
    {
        var from = update.Message?.From;
        var chatId = $"Chat Id: {chat.Id}";
        var chatType = $"Chat Type: {chat.Type}";
        var fromUser = $"From User: {from?.FirstName} {from?.LastName} @{from?.Username} id:{from?.Id}";

        var info = new[] { chatId, chatType, fromUser };
        for (var i = 0; i < info.Length; i++) info[i] = Regex.Replace(info[i], @"\s+", " ").Trim();

        return string.Join("\n", info);
    }
}