using Bot.GetByLink.Client.Telegram.Polling.Interfaces;
using Bot.GetByLink.Common.Infrastructure;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling;

/// <summary>
///     Class for formating content for messages telegram.
/// </summary>
public class FormaterContentTelegram : IFormaterContentTelegram
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FormaterContentTelegram" /> class.
    /// </summary>
    /// <param name="responseContent">Proxy content.</param>
    public FormaterContentTelegram()
    {
        // TODO Потестить отдельно гифки
        AlbumInputMedias = new List<IAlbumInputMedia>();
        Messages = new List<string>();
        MaxSizeMbPhoto = 5;
        MaxSizeMbVideo = 20;
        MaxTextLenghtFirstMedia = 1024;
        MaxTextLenghtMessage = 4096;
    }

    /// <summary>
    ///     Gets text messages.
    /// </summary>
    public IEnumerable<string> Messages { get; private set; }

    /// <summary>
    ///     Gets media messages.
    /// </summary>
    public IEnumerable<IAlbumInputMedia> AlbumInputMedias { get; private set; }

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
    public void SetFormaterContent(ProxyResponseContent responseContent)
    {
        var (textUrl, urlPictures, urlVideo) =
            GetTextUrlAndValidUrl(responseContent.UrlPicture.ToList(), responseContent.UrlPicture.ToList());
        var hasMedia = urlPictures.Count > 0 || urlVideo.Count > 0;
        var textMessage = $"{textUrl}{responseContent.Text}";
        var captionLength = hasMedia ? Math.Min(MaxTextLenghtFirstMedia, textMessage.Length) : 0;
        var captionMedia = textMessage[..captionLength];
        var albumInputMedias = GetPhotoInputMedia(urlPictures, captionMedia);
        albumInputMedias.AddRange(GetVideoInputMedia(urlVideo, captionMedia, albumInputMedias.Count < 1));
        AlbumInputMedias = albumInputMedias;
        Messages = GetMessageArray(textMessage, captionLength);
    }

    /// <summary>
    ///     Function for get formating list IAlbumInputMedia from array picture.
    /// </summary>
    /// <param name="urlPictures">Array url picture.</param>
    /// <returns>Formating list IAlbumInputMedia.</returns>
    private static List<IAlbumInputMedia> GetPhotoInputMedia(List<MediaInfo> urlPictures, string captionMedia)
    {
        var albumInputMedias = new List<IAlbumInputMedia>();
        if (urlPictures.Count > 0)
        {
            var listInputMediaPhoto = new List<InputMediaPhoto>();
            foreach (var inputMediaPhoto in urlPictures)
                listInputMediaPhoto.Add(new InputMediaPhoto(inputMediaPhoto.UrlMedia));

            listInputMediaPhoto[0].Caption = captionMedia;
            albumInputMedias = listInputMediaPhoto.ToList<IAlbumInputMedia>();
        }

        return albumInputMedias;
    }

    /// <summary>
    ///     Function for get formating list IAlbumInputMedia from array video.
    /// </summary>
    /// <param name="urlVideo">Array url video.</param>
    /// <returns>Formating list IAlbumInputMedia.</returns>
    private static List<IAlbumInputMedia> GetVideoInputMedia(List<MediaInfo> urlVideo, string captionMedia,
        bool isFirstMedia)
    {
        var albumInputMedias = new List<IAlbumInputMedia>();
        if (urlVideo.Count > 0)
        {
            var listInputMediaVideo = new List<InputMediaVideo>();
            foreach (var inputMediaVideo in urlVideo)
                listInputMediaVideo.Add(new InputMediaVideo(inputMediaVideo.UrlMedia));

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
    private (string MutableUrlText, List<MediaInfo> MutableUrlPicture, List<MediaInfo> MutableUrlVideo)
        GetTextUrlAndValidUrl(
            List<MediaInfo> mediaPicture, List<MediaInfo> mediaVideo)
    {
        var urlText = string.Empty;
        var mutableUrlPicture = new List<MediaInfo>();
        var mutableUrlVideo = new List<MediaInfo>();
        foreach (var inputMediaPhoto in mediaPicture)
            if (inputMediaPhoto.SizeMedia < 0 || inputMediaPhoto.SizeMedia > MaxSizeMbPhoto)
                urlText = $"{inputMediaPhoto.UrlMedia}\n{urlText}";
            else
                mutableUrlPicture.Add(inputMediaPhoto);

        foreach (var inputMediaVideo in mediaVideo)
            if (inputMediaVideo.SizeMedia < 0 || inputMediaVideo.SizeMedia > MaxSizeMbVideo)
                urlText = $"{inputMediaVideo.UrlMedia}\n{urlText}";
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
    private List<string> GetMessageArray(string textPost, int captionLength)
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