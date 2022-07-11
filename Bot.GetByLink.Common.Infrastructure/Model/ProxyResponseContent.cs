namespace Bot.GetByLink.Common.Infrastructure.Model;

/// <summary>
///     TODO: temporary implementation. Сlass for returned object from proxy.
/// </summary>
public class ProxyResponseContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyResponseContent" /> class.
    /// </summary>
    /// <param name="text">Text from post.</param>
    /// <param name="urlPicture">Array url pictures from post.</param>
    /// <param name="urlVideo">Array url video from post.</param>
    public ProxyResponseContent(string text, MediaInfo[] urlPicture, MediaInfo[] urlVideo)
    {
        Text = text;
        UrlPicture = urlPicture;
        UrlVideo = urlVideo;
    }

    /// <summary>
    ///     Gets текст поста.
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     Gets картики в посте.
    /// </summary>
    public MediaInfo[] UrlPicture { get; }

    /// <summary>
    ///     Gets видео в посте.
    /// </summary>
    public MediaInfo[] UrlVideo { get; }
}