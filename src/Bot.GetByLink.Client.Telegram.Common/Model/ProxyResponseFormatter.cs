using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Model;
using Bot.GetByLink.Common.Interfaces.Configuration.Clients;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

// TODO: test with gif

/// <summary>
///     Class for formating content for messages telegram.
/// </summary>
public class ProxyResponseFormatter : IFormatterContent
{
    /// <summary>
    ///     Function get messages from proxy content.
    /// </summary>
    /// <param name="proxyContent">Proxy content.</param>
    /// <param name="setHeader">Set header in messages.</param>
    /// <param name="url">Url post in messages.</param>
    /// <param name="configuration">Configuration telegram.</param>
    /// <returns>List messages.</returns>
    public (IEnumerable<string> Messages, IEnumerable<IEnumerable<IAlbumInputMedia>> Artifacts) GetListMessages(
        IProxyContent proxyContent, bool setHeader, string url, ITelegramConfiguration configuration)
    {
        var (textUrl, urlPictures, urlVideo) =
            GetTextUrlAndValidUrl(proxyContent.UrlPicture, proxyContent.UrlVideo, configuration);
        var textContent =
            $"{(url.Length > 0 ? $"{url}\n\n" : string.Empty)}{(textUrl.Length > 0 ? $"{textUrl}\n" : string.Empty)}{(setHeader && proxyContent.Header?.Length > 0 ? $"{proxyContent.Header}\n\n" : string.Empty)}{proxyContent.Text}";
        var listTextMessages = GetListTextMessages(textContent, urlPictures.Count() + urlVideo.Count(), configuration);
        var listInputMedia = GetListInputMedia(urlPictures.Concat(urlVideo), listTextMessages, configuration);
        return (listTextMessages.Skip(urlPictures.Count() + urlVideo.Count()), listInputMedia);
    }

    /// <summary>
    ///     Get list input media for messages.
    /// </summary>
    /// <param name="listMedia">List media.</param>
    /// <param name="textMessages">Text for media.</param>
    /// <param name="configuration">Configuration telegram.</param>
    /// <returns>List input media for messages.</returns>
    private static IEnumerable<IEnumerable<IAlbumInputMedia>> GetListInputMedia(
        IEnumerable<IMediaInfo> listMedia,
        IEnumerable<string> textMessages,
        ITelegramConfiguration configuration)
    {
        var listPhotoMedias = listMedia.Where(x => x.Type == MediaType.Photo)
            .Select<IMediaInfo, InputMediaBase>(media => new InputMediaPhoto(media.Url));
        var listVideoMedias = listMedia.Where(x => x.Type == MediaType.Video || x.Type == MediaType.Gif)
            .Select<IMediaInfo, InputMediaBase>(media => new InputMediaVideo(media.Url));
        
        var groupListFormatedMedias = listPhotoMedias
            .Split(configuration.MaxColMediaInMessage)
            .Select(x => x.ToList())
            .Concat(listVideoMedias
                .Split(configuration.MaxColMediaInMessage)
                .Select(x => x.ToList()))
            .ToList();

        for (var i = 0; i < groupListFormatedMedias.Count; i++)
        {
            var itemMedias = groupListFormatedMedias[i];
            var firstItemMedia = itemMedias[0];
            if (firstItemMedia != null) firstItemMedia.Caption = textMessages.Skip(i).FirstOrDefault();
        }

        return groupListFormatedMedias.Select(x => x.Select(y => y as IAlbumInputMedia));
    }

    /// <summary>
    ///     Get list text for messages.
    /// </summary>
    /// <param name="allText">Text in content.</param>
    /// <param name="countMediaMessage">Count media in content.</param>
    /// <param name="configuration">Configuration telegram.</param>
    /// <returns>List text for messages.</returns>
    private static IEnumerable<string> GetListTextMessages(
        string allText,
        int countMediaMessage,
        ITelegramConfiguration configuration)
    {
        if ((countMediaMessage == 0 && allText.Length < configuration.MaxTextLenghtMessage) ||
            (countMediaMessage > 0 && allText.Length < configuration.MaxTextLenghtFirstMedia))
            return new List<string> { allText };
        var finderTextMessage = allText;
        var messageText = new List<string>();
        while (finderTextMessage.Length > 0)
        {
            var maxSizeMessage = messageText.Count < countMediaMessage
                ? configuration.MaxTextLenghtFirstMedia
                : configuration.MaxTextLenghtMessage;
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
    /// <param name="configuration">Configuration telegram.</param>
    /// <returns>Text with url and array valid url.</returns>
    private static (string MutableUrlText, IEnumerable<IMediaInfo> MutableUrlPicture, IEnumerable<IMediaInfo>
        MutableUrlVideo)
        GetTextUrlAndValidUrl(
            IEnumerable<IMediaInfo>? mediaPicture,
            IEnumerable<IMediaInfo>? mediaVideo,
            ITelegramConfiguration configuration)
    {
        var urlText = string.Empty;
        var mutableUrlPicture = new List<IMediaInfo>();
        var mutableUrlVideo = new List<IMediaInfo>();

        if (mediaPicture?.Any() ?? false)
            foreach (var inputMediaPhoto in mediaPicture)
                if (inputMediaPhoto.Size < 0
                    || inputMediaPhoto.Size >= configuration.MaxSizeMbPhoto
                    || inputMediaPhoto.Width > configuration.MaxSizePxMedia
                    || inputMediaPhoto.Height > configuration.MaxSizePxMedia)
                    urlText = $"{inputMediaPhoto.Url}\n{urlText}";
                else mutableUrlPicture.Add(inputMediaPhoto);

        if (mediaVideo?.Any() ?? false)
            foreach (var inputMediaVideo in mediaVideo)
                if (inputMediaVideo.Size < 0
                    || inputMediaVideo.Size >= configuration.MaxSizeMbVideo
                    || inputMediaVideo.Width > configuration.MaxSizePxMedia
                    || inputMediaVideo.Height > configuration.MaxSizePxMedia)
                    urlText = $"{inputMediaVideo.Url}\n{urlText}";
                else mutableUrlVideo.Add(inputMediaVideo);

        return (urlText, mutableUrlPicture, mutableUrlVideo);
    }
}