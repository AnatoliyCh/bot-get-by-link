namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Interface for returned object from proxy.
/// </summary>
internal interface IProxyContent
{
    /// <summary>
    ///     Gets текст поста.
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    ///     Gets картики в посте.
    /// </summary>
    public IMediaInfo[] UrlPicture { get; init; }

    /// <summary>
    ///     Gets видео в посте.
    /// </summary>
    public IMediaInfo[] UrlVideo { get; init; }
}