using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Bot.GetByLink.Client.Telegram.Common.Model.Message;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Returns content post Reddit.
/// </summary>
internal sealed class SendContentFromUrlCommand : AsyncCommand<CommandName>
{
    private readonly IAsyncCommand<CommandName> sendMessageCommand;
    private readonly IRegexWrapper urlRegex;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendContentFromUrlCommand" /> class.
    /// </summary>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    /// <param name="proxyServices">Proxy collection.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    public SendContentFromUrlCommand(
        IAsyncCommand<CommandName> sendMessageCommand,
        IEnumerable<IProxyService> proxyServices,
        IEnumerable<IRegexWrapper> regexWrappers)
        : base(CommandName.SendContentFromUrl)
    {
        this.sendMessageCommand = sendMessageCommand ?? throw new ArgumentNullException(nameof(sendMessageCommand));
        ProxyServices = proxyServices ?? throw new ArgumentNullException(nameof(proxyServices));
        urlRegex = GetUrlRegexWrapperByIRegexWrappers(regexWrappers) ??
                   throw new ArgumentNullException(nameof(regexWrappers));
    }

    /// <summary>
    ///     Gets a collection of proxy services.
    /// </summary>
    public IEnumerable<IProxyService> ProxyServices { get; }

    /// <summary>
    ///     //TODO RENAME: Collect and send content post reddit.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not Update update) return;
        var chatId = update.Message?.Chat.Id;
        var text = update.Message?.Text;
        if (chatId is null || string.IsNullOrWhiteSpace(text)) return;

        var url = urlRegex.Match(text)?.Value;
        if (string.IsNullOrWhiteSpace(url)) return;

        var matchProxy = ProxyServices.FirstOrDefault(proxy => proxy.IsMatch(url));
        if (matchProxy is null) return;

        var postContent = await matchProxy.GetContentUrl(url);
        if (postContent is null) return;

        // TODO: add foormatter
        var sendText = new List<string> { postContent.Text ?? string.Empty };
        var artifacts = new List<string>
            { postContent.UrlPicture ?? string.Empty, postContent.UrlVideo ?? string.Empty };
        var message = new Message(chatId, sendText, artifacts, ParseMode.MarkdownV2);
        await sendMessageCommand.ExecuteAsync(message);
    }

    private static UrlRegexWrapper? GetUrlRegexWrapperByIRegexWrappers(IEnumerable<IRegexWrapper>? regexWrappers)
    {
        if (regexWrappers is null || !regexWrappers.Any()) return null;
        foreach (var regex in regexWrappers)
            if (regex is UrlRegexWrapper urlRegex)
                return urlRegex;

        return null;
    }
}