using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Returns content post Reddit.
/// </summary>
public sealed class SendContentFromUrlCommand : AsyncCommand<CommandName>
{
    private readonly IBuilderMessage builderMessage;
    private readonly IAsyncCommand<CommandName> sendMessageCommand;
    private readonly IRegexWrapper urlRegex;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendContentFromUrlCommand" /> class.
    /// </summary>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    /// <param name="proxyServices">Proxy collection.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    /// <param name="builderMessage">Builder message.</param>
    public SendContentFromUrlCommand(
        IAsyncCommand<CommandName> sendMessageCommand,
        IEnumerable<IProxyService> proxyServices,
        IEnumerable<IRegexWrapper> regexWrappers,
        IBuilderMessage builderMessage)
        : base(CommandName.SendContentFromUrl)
    {
        this.sendMessageCommand = sendMessageCommand ?? throw new ArgumentNullException(nameof(sendMessageCommand));
        this.builderMessage = builderMessage;
        ProxyServices = proxyServices ?? throw new ArgumentNullException(nameof(proxyServices));
        urlRegex = GetUrlRegexWrapperByIRegexWrappers(regexWrappers) ??
                   throw new ArgumentNullException(nameof(regexWrappers));
    }

    /// <summary>
    ///     Gets a collection of proxy services.
    /// </summary>
    public IEnumerable<IProxyService> ProxyServices { get; }

    /// <summary>
    ///     Collect and send content post.
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

        var postContent = await matchProxy.GetContentUrlAsync(url);
        if (postContent is null) return;

        var message = builderMessage
            .From(postContent)
            .AddUrl(url)
            .AddChatId(chatId ?? -1)
            .SetHeaders()
            .SetParseMode(ParseMode.MarkdownV2)
            .Build();

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