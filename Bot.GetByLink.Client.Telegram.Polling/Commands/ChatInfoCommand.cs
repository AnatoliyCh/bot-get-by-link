using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Returns information about the current chat and the sender.
/// </summary>
internal class ChatInfoCommand : Command<CommandName>
{
    private readonly ITelegramBotClient client;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatInfoCommand" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    /// <param name="client">Telegram Client.</param>
    public ChatInfoCommand(CommandName name, ITelegramBotClient client)
        : base(name)
    {
        this.client = client;
    }

    /// <summary>
    ///     Collect and send chat information.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task Execute(object? ctx = null)
    {
        if (ctx is not Update) return;
        var update = ctx as Update;
        var chatId = update?.Message?.Chat.Id;
        if (update is null || chatId is null) return;

        var cts = new CancellationTokenSource();
        var chat = await client.GetChatAsync(chatId, cts.Token);
        var info = GetInfoByUpdate(chat, update);
        await client.SendTextMessageAsync(chatId, info, cancellationToken: cts.Token);
    }

    private string GetInfoByUpdate(Chat chat, Update update)
    {
        var from = update.Message?.From;
        var chatId = $"Chat Id: {chat.Id}";
        var chatType = $"Chat Type: {chat.Type}";
        var fromUser = $"From User: {from?.FirstName} {from?.LastName} @{from?.Username}";

        var info = new[] { chatId, chatType, fromUser };
        for (var i = 0; i < info.Length; i++) info[i] = Regex.Replace(info[i], @"\s+", " ").Trim();

        return string.Join("\n", info);
    }
}