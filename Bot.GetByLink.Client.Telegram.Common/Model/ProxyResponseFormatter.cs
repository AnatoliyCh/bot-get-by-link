using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model;
using Bot.GetByLink.Common.Infrastructure.Model.Configuration.Clients;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model;
// TODO: test with gif
/// <summary>
///     Class for formating content for messages telegram.
/// </summary>
public class ProxyResponseFormatter : IFormatterContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyResponseFormatter" /> class.
    /// </summary>
    /// <param name="configuration">Client Configuration.</param>
    public ProxyResponseFormatter(ClientConfiguration configuration)
    {
        MaxSizeMbPhoto = configuration.MaxSizeMbPhoto;
        MaxSizeMbVideo = configuration.MaxSizeMbVideo;
        MaxTextLenghtFirstMedia = configuration.MaxTextLenghtFirstMedia;
        MaxTextLenghtMessage = configuration.MaxTextLenghtMessage;
    }

    /// <summary>
    ///     Gets max size in mb photo for telegram bot.
    /// </summary>
    public double MaxSizeMbPhoto { get; }

    /// <summary>
    ///     Gets max size in mb vide telegram bot.
    /// </summary>
    public double MaxSizeMbVideo { get; }

    /// <summary>
    ///     Gets max text length first media.
    /// </summary>
    public int MaxTextLenghtFirstMedia { get; }

    /// <summary>
    ///     Gets max text length in message.
    /// </summary>
    public int MaxTextLenghtMessage { get; }

    /// <summary>
    ///     Function for set content.
    /// </summary>
    /// <param name="responseContent">Proxy content.</param>
    /// <returns>Formatted content.</returns>
    public (IEnumerable<string> Messages, IEnumerable<IAlbumInputMedia> Artifacts) GetFormattedContent(
        ProxyResponseContent responseContent)
    {
        var (textUrl, urlPictures, urlVideo) =
            GetTextUrlAndValidUrl(responseContent.UrlPicture.ToList(), responseContent.UrlVideo.ToList());
        var hasMedia = urlPictures.Any() || urlVideo.Any();
        var textMessage = $"{textUrl}{responseContent.Text}";
        var captionLength = hasMedia ? Math.Min(MaxTextLenghtFirstMedia, textMessage.Length) : 0;
        var captionMedia = textMessage[..captionLength];
        var albumInputMedias = GetPhotoInputMedia(urlPictures, captionMedia);
        albumInputMedias = albumInputMedias.Concat(GetVideoInputMedia(urlVideo, captionMedia, !albumInputMedias.Any()));
        return (GetMessageList(textMessage, captionLength), albumInputMedias);
    }

    /// <summary>
    ///     Function for get formating list IAlbumInputMedia from array picture.
    /// </summary>
    /// <param name="urlPictures">Array url picture.</param>
    /// <param name="captionMedia">Caption picture.</param>
    /// <returns>Formating list IAlbumInputMedia.</returns>
    private static IEnumerable<IAlbumInputMedia> GetPhotoInputMedia(IEnumerable<IMediaInfo> urlPictures,
        string captionMedia)
    {
        var albumInputMedias = new List<IAlbumInputMedia>();
        if (urlPictures.Any())
        {
            var listInputMediaPhoto = new List<InputMediaPhoto>();
            foreach (var inputMediaPhoto in urlPictures)
                listInputMediaPhoto.Add(new InputMediaPhoto(inputMediaPhoto.Url));

            listInputMediaPhoto[0].Caption = captionMedia;
            albumInputMedias = listInputMediaPhoto.ToList<IAlbumInputMedia>();
        }

        return albumInputMedias;
    }

    /// <summary>
    ///     Function for get formating list IAlbumInputMedia from array video.
    /// </summary>
    /// <param name="urlVideo">Array url video.</param>
    /// <param name="captionMedia">Caption video.</param>
    /// <param name="isFirstMedia">True if this first media.</param>
    /// <returns>Formating list IAlbumInputMedia.</returns>
    private static IEnumerable<IAlbumInputMedia> GetVideoInputMedia(IEnumerable<IMediaInfo> urlVideo,
        string captionMedia,
        bool isFirstMedia)
    {
        var albumInputMedias = new List<IAlbumInputMedia>();
        if (urlVideo.Any())
        {
            var listInputMediaVideo = new List<InputMediaVideo>();
            foreach (var inputMediaVideo in urlVideo)
                listInputMediaVideo.Add(new InputMediaVideo(inputMediaVideo.Url));

            if (isFirstMedia) listInputMediaVideo[0].Caption = captionMedia;

            albumInputMedias.AddRange(listInputMediaVideo.ToList<IAlbumInputMedia>());
        }

        return albumInputMedias;
    }

    /// <summary>
    ///     Function get text url that much max size (5mb pic and 20 video) and valid url.
    /// </summary>
    /// <param name="mediaPicture">Array media info pictures.</param>
    /// <param name="mediaVideo">Array media info viedos.</param>
    /// <returns>Text with url and array valid url.</returns>
    private (string MutableUrlText, IEnumerable<IMediaInfo> MutableUrlPicture, IEnumerable<IMediaInfo> MutableUrlVideo)
        GetTextUrlAndValidUrl(
            List<IMediaInfo> mediaPicture, List<IMediaInfo> mediaVideo)
    {
        var urlText = string.Empty;
        var mutableUrlPicture = new List<IMediaInfo>();
        var mutableUrlVideo = new List<IMediaInfo>();
        foreach (var inputMediaPhoto in mediaPicture)
            if (inputMediaPhoto.Size < 0 || inputMediaPhoto.Size > MaxSizeMbPhoto)
                urlText = $"{inputMediaPhoto.Url}\n{urlText}";
            else
                mutableUrlPicture.Add(inputMediaPhoto);

        foreach (var inputMediaVideo in mediaVideo)
            if (inputMediaVideo.Size < 0 || inputMediaVideo.Size > MaxSizeMbVideo)
                urlText = $"{inputMediaVideo.Url}\n{urlText}";
            else
                mutableUrlVideo.Add(inputMediaVideo);

        return (urlText, mutableUrlPicture, mutableUrlVideo);
    }

    /// <summary>
    ///     Function for get formating message array..
    /// </summary>
    /// <param name="textPost">Text post.</param>
    /// <param name="captionLength">Lenght text caption picture or video.</param>
    /// <returns>Formating list IAlbumInputMedia.</returns>
    private List<string> GetMessageList(string textPost, int captionLength)
    {
        var messages = new List<string>();
        if (textPost.Length > captionLength)
        {
            textPost = textPost[captionLength..];
            for (var i = 0; textPost.Length > MaxTextLenghtMessage; i++)
            {
                messages.Add(textPost.Remove(MaxTextLenghtMessage));
                textPost = textPost[MaxTextLenghtMessage..];
            }

            messages.Add(textPost);
        }

        return messages;
    }
}