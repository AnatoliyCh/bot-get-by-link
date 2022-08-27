using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces.Configuration.Clients;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     Class for build message.
/// </summary>
public sealed class BuilderMessage : IBuilderMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BuilderMessage" /> class.
    /// </summary>
    /// <param name="configuration">Config for builder messages.</param>
    public BuilderMessage(ITelegramConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        Configuration = configuration;
        FormatterContent = new ProxyResponseFormatter();
    }

    /// <summary>
    ///     Gets or sets content for which the message is built.
    /// </summary>
    private IProxyContent ProxyContent { get; set; } = new ProxyResponseContent(string.Empty);

    /// <summary>
    ///     Gets object for formating content for messages telegram.
    /// </summary>
    private IFormatterContent FormatterContent { get; }

    /// <summary>
    ///     Gets or sets url where the content get from.
    /// </summary>
    private string Url { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets chat ID where messages will be sent.
    /// </summary>
    private long ChatId { get; set; } = -1;

    /// <summary>
    ///     Gets or sets parse mode for right parsing content.
    /// </summary>
    private ParseMode? ParseMode { get; set; }

    /// <summary>
    ///     Gets parse mode for right parsing content.
    /// </summary>
    private ITelegramConfiguration Configuration { get; }

    /// <summary>
    ///     Gets or sets a value indicating whether header in text message.
    /// </summary>
    private bool SetHeader { get; set; }

    /// <summary>
    ///     From conter build message.
    /// </summary>
    /// <param name="proxyContent">Content.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage From(IProxyContent proxyContent)
    {
        ProxyContent = proxyContent;
        return this;
    }

    /// <summary>
    ///     Add url in builder.
    /// </summary>
    /// <param name="url">Url.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage AddUrl(string url)
    {
        Url = url;
        return this;
    }

    /// <summary>
    ///     Set header in builder.
    /// </summary>
    /// <returns>This builder.</returns>
    public IBuilderMessage SetHeaders()
    {
        SetHeader = true;
        return this;
    }

    /// <summary>
    ///     Add chat id in builder.
    /// </summary>
    /// <param name="chatId">Chat id.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage AddChatId(long chatId)
    {
        ChatId = chatId;
        return this;
    }

    /// <summary>
    ///     Set parse mode id in builder.
    /// </summary>
    /// <param name="parseMode">Parse mode.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage SetParseMode(ParseMode parseMode)
    {
        ParseMode = parseMode;
        return this;
    }

    /// <summary>
    ///     Build messages.
    /// </summary>
    /// <returns>Ready messages.</returns>
    public IMessageContext Build()
    {
        var (messages, artifacts) = FormatterContent.GetListMessages(ProxyContent, SetHeader, Url, Configuration);
        var message = new Message(ChatId, messages, artifacts, ParseMode);
        SetDefaultParametersBuilder();
        return message;
    }

    /// <summary>
    ///     Set deafault parameters in builder.
    /// </summary>
    private void SetDefaultParametersBuilder()
    {
        Url = string.Empty;
        ChatId = -1;
        ParseMode = null;
        SetHeader = false;
        ProxyContent = new ProxyResponseContent(string.Empty);
    }
}