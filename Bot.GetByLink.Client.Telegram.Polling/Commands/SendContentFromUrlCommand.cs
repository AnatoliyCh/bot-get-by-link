using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Proxy.Reddit;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Returns content post Reddit.
/// </summary>
internal class SendContentFromUrlCommand : AsyncCommand<CommandName>
{
    private readonly ITelegramBotClient client;

    private readonly string patternURL =
        @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendContentFromUrlCommand" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    /// <param name="client">Telegram Client.</param>
    public SendContentFromUrlCommand(CommandName name, ITelegramBotClient client)
        : base(name)
    {
        var proxyReddit = new ProxyReddit(new[] { @"https?:\/\/www.reddit.com\/r\/\S+/comments\/\S+" });
        ProxyServices = new List<IProxyService> { proxyReddit };
        this.client = client;
    }

    /// <summary>
    ///     Gets list proxy api.
    /// </summary>
    public List<IProxyService> ProxyServices { get; }

    /// <summary>
    ///     Collect and send content post reddit.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not Update) return;
        var update = ctx as Update;
        var chatId = update?.Message?.Chat.Id;
        var text = update?.Message?.Text;

        if (update is null || chatId is null || string.IsNullOrWhiteSpace(text)) return;
        var url = Regex.Match(text, patternURL).Value;

        if (string.IsNullOrWhiteSpace(url)) return;
        var matchProxy = ProxyServices.FirstOrDefault(x => x.IsMatch(url));

        if (matchProxy == null) return;
        var postContent = await matchProxy.GetContentUrlAsync(url);
        if (postContent is null) return;
        var cts = new CancellationTokenSource();

        var content = new FormaterMessage(postContent);

        if (content.AlbumInputMedias.Count > 0)
        {
            await client.SendMediaGroupAsync(chatId, content.AlbumInputMedias, cancellationToken: cts.Token);
        }

        if (content.Messages.Count > 0)
        {
            foreach (var message in content.Messages)
            {
                await client.SendTextMessageAsync(chatId, message, cancellationToken: cts.Token);
            }
        }
    }
}