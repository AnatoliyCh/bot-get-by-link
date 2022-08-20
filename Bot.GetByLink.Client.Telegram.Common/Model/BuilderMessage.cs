using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model;
using Bot.GetByLink.Common.Infrastructure.Model.Configuration.Clients;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     Class for build message.
/// </summary>
public sealed class BuilderMessage : IBuilderMessage
{
    /// <summary>
    ///     Content for which the message is built.
    /// </summary>
    private IProxyContent proxyContent;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuilderMessage" /> class.
    /// </summary>
    /// <param name="configuration">Config for builder messages.</param>
    public BuilderMessage(ClientConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        MaxSizeMbPhoto = configuration.MaxSizeMbPhoto;
        MaxSizeMbVideo = configuration.MaxSizeMbVideo;
        MaxTextLenghtFirstMedia = configuration.MaxTextLenghtFirstMedia;
        MaxTextLenghtMessage = configuration.MaxTextLenghtMessage;
        MaxColMediaInMessage = configuration.MaxColMediaImMessage;
        SetDefaultParametersBuilder();
    }

    /// <summary>
    ///     Gets or sets url where the content get from.
    /// </summary>
    private string Url { get; set; }

    /// <summary>
    ///     Gets or sets chat ID where messages will be sent.
    /// </summary>
    private long ChatId { get; set; }

    /// <summary>
    ///     Gets or sets parse mode for right parsing content.
    /// </summary>
    private ParseMode ParseMode { get; set; }

    /// <summary>
    ///     Gets max size in mb photo for telegram bot.
    /// </summary>
    private double MaxSizeMbPhoto { get; }

    /// <summary>
    ///     Gets max size in mb vide telegram bot.
    /// </summary>
    private double MaxSizeMbVideo { get; }

    /// <summary>
    ///     Gets max text length first media.
    /// </summary>
    private int MaxTextLenghtFirstMedia { get; }

    /// <summary>
    ///     Gets max text length in message.
    /// </summary>
    private int MaxTextLenghtMessage { get; }

    /// <summary>
    ///     Gets max col media in message.
    /// </summary>
    private int MaxColMediaInMessage { get; }

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
        this.proxyContent = proxyContent;
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
        var (messages, artifacts) = GetFormattedContent();
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
        ParseMode = ParseMode.MarkdownV2;
        SetHeader = false;
        proxyContent = new ProxyResponseContent(string.Empty);
    }

    /// <summary>
    ///     Function for set content.
    /// </summary>
    /// <returns>Formatted content.</returns>
    private (IEnumerable<string> Messages, IEnumerable<IEnumerable<IAlbumInputMedia>> Artifacts) GetFormattedContent()
    {
        var (textUrl, urlPictures, urlVideo) =
            GetTextUrlAndValidUrl(proxyContent.UrlPicture, proxyContent.UrlVideo);
        var textContent = $"{Url}\n{textUrl}\n{(SetHeader ? proxyContent.Header : string.Empty)}\n{proxyContent.Text}";
        var listTextMessages = GetListTextMessages(textContent, urlPictures.Count() + urlVideo.Count());
        var listInputMedia = GetListInputMedia(urlPictures.Concat(urlVideo), listTextMessages);
        return (listTextMessages.Skip(urlPictures.Count() + urlVideo.Count()), listInputMedia);
    }

    /// <summary>
    ///     Get list input media for messages.
    /// </summary>
    /// <param name="listMedia">List media.</param>
    /// <param name="textMessages">Text for media.</param>
    /// <returns>List input media for messages.</returns>
    private IEnumerable<IEnumerable<IAlbumInputMedia>> GetListInputMedia(IEnumerable<IMediaInfo> listMedia,
        IEnumerable<string> textMessages)
    {
        var listFormatedMedias = listMedia.Select<IMediaInfo, InputMediaBase>((media, index) =>
        {
            return media.Type == MediaType.Photo ? new InputMediaPhoto(media.Url) : new InputMediaVideo(media.Url);
        });

        var groupListFormatedMedias = listFormatedMedias.Split(MaxColMediaInMessage).Select(x => x.ToList()).ToList();

        for (var i = 0; i < groupListFormatedMedias.Count; i++)
        {
            var itemMedias = groupListFormatedMedias[i];
            var firstItemMedia = itemMedias[0];
            if (firstItemMedia != null) firstItemMedia.Caption = textMessages.Skip(i).Take(1).FirstOrDefault();
        }

        return groupListFormatedMedias.Select(x => x.Select(y => y as IAlbumInputMedia));
    }

    /// <summary>
    ///     Get list text for messages.
    /// </summary>
    /// <param name="allText">Text in content.</param>
    /// <param name="countMediaMessage">Count media in content.</param>
    /// <returns>List text for messages.</returns>
    private IEnumerable<string> GetListTextMessages(string allText, int countMediaMessage)
    {
        if ((countMediaMessage == 0 && allText.Length < MaxTextLenghtMessage) ||
            (countMediaMessage > 0 && allText.Length < MaxTextLenghtFirstMedia)) return new List<string> { allText };
        var finderTextMessage = allText;
        var messageText = new List<string>();
        while (finderTextMessage.Length > 0)
        {
            var maxSizeMessage = messageText.Count < countMediaMessage ? MaxTextLenghtFirstMedia : MaxTextLenghtMessage;
            var sizeCurrentMessage =
                finderTextMessage.Length > maxSizeMessage ? maxSizeMessage : finderTextMessage.Length;
            var textMessage = finderTextMessage[..sizeCurrentMessage];
            var intedDot = textMessage.LastIndexOf(". ");
            var intedParagraphEndSentecis = textMessage.LastIndexOf(".\n");
            var intedParagraph = intedParagraphEndSentecis == -1
                ? textMessage.LastIndexOf("\n")
                : intedParagraphEndSentecis;
            var intedCut = (intedParagraph == -1 ? intedDot : intedParagraph) + 1;
            if (maxSizeMessage > finderTextMessage.Length) intedCut = finderTextMessage.Length;
            messageText.Add(finderTextMessage[..intedCut]);
            finderTextMessage = finderTextMessage[intedCut..];
        }

        return messageText;
    }

    /// <summary>
    ///     Function get text url that much max size (5mb pic and 20 video) and valid url.
    /// </summary>
    /// <param name="mediaPicture">Array media info pictures.</param>
    /// <param name="mediaVideo">Array media info viedos.</param>
    /// <returns>Text with url and array valid url.</returns>
    private (string MutableUrlText, IEnumerable<IMediaInfo> MutableUrlPicture, IEnumerable<IMediaInfo> MutableUrlVideo)
        GetTextUrlAndValidUrl(IEnumerable<IMediaInfo>? mediaPicture, IEnumerable<IMediaInfo>? mediaVideo)
    {
        var urlText = string.Empty;
        var mutableUrlPicture = new List<IMediaInfo>();
        var mutableUrlVideo = new List<IMediaInfo>();

        if (mediaPicture?.Any() ?? false)
            foreach (var inputMediaPhoto in mediaPicture)
                if (inputMediaPhoto.Size < 0 || inputMediaPhoto.Size > MaxSizeMbPhoto)
                    urlText = $"{inputMediaPhoto.Url}\n{urlText}";
                else mutableUrlPicture.Add(inputMediaPhoto);

        if (mediaVideo?.Any() ?? false)
            foreach (var inputMediaVideo in mediaVideo)
                if (inputMediaVideo.Size < 0 || inputMediaVideo.Size > MaxSizeMbVideo)
                    urlText = $"{inputMediaVideo.Url}\n{urlText}";
                else mutableUrlVideo.Add(inputMediaVideo);

        return (urlText, mutableUrlPicture, mutableUrlVideo);
    }
}