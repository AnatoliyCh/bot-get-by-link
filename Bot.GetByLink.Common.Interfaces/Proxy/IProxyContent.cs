namespace Bot.GetByLink.Common.Interfaces.Proxy;

/// <summary>
///     Interface for returned object from proxy.
/// </summary>
public interface IProxyContent
{
    /// <summary>
    ///     Gets текст поста.
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    ///     Gets картики в посте.
    /// </summary>
    public IEnumerable<IMediaInfo>? UrlPicture { get; init; }

    /// <summary>
    ///     Gets видео в посте.
    /// </summary>
    public IEnumerable<IMediaInfo>? UrlVideo { get; init; }
}