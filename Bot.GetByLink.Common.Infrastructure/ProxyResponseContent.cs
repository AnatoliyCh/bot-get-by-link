namespace Bot.GetByLink.Common.Infrastructure;

/// <summary>
///     TODO: temporary implementation. Сlass for returned object from proxy.
/// </summary>
public class ProxyResponseContent
{
    /// <summary>
    ///     Gets or sets текст поста.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Gets or sets картики в посте.
    /// </summary>
    public string[] UrlPicture { get; set; }

    /// <summary>
    ///     Gets or sets видео в посте.
    /// </summary>
    public string[] UrlVideo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyResponseContent"/> class.
    /// </summary>
    /// <param name="text">Text from post.</param>
    /// <param name="urlPicture">Array url pictures from post.</param>
    /// <param name="urlVideo">Array url video from post.</param>
    public ProxyResponseContent(string text, string[] urlPicture, string[] urlVideo)
    {
        Text = text;
        UrlPicture = urlPicture;
        UrlVideo = urlVideo;
    }
}