namespace Bot.GetByLink.Common.Infrastructure;

/// <summary>
/// TODO: temporary implementation. Сlass for returned object from proxy.
/// </summary>
public class ProxyResponseContent
{
    /// <summary>
    ///     Gets or sets текст поста.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Gets or sets картики в посте.
    /// </summary>
    public string? UrlPicture { get; set; }

    /// <summary>
    ///     Gets or sets видео в посте.
    /// </summary>
    public string? UrlVideo { get; set; }
}