namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Interface for returned object from proxy.
/// </summary>
public interface IProxyContent
{
    /// <summary>
    ///     Gets text post.
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    ///     Gets header post.
    /// </summary>
    public string? Header { get; init; }

    /// <summary>
    ///     Gets picture post.
    /// </summary>
    public IEnumerable<IMediaInfo>? UrlPicture { get; init; }

    /// <summary>
    ///     Gets video post.
    /// </summary>
    public IEnumerable<IMediaInfo>? UrlVideo { get; init; }
}