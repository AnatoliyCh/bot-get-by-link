using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Proxy.Vk.Interfaces;

/// <summary>
///     Content return strategies interface.
/// </summary>
public interface IContentReturnStrategy
{
    /// <summary>
    ///     Return content by url.
    /// </summary>
    /// <param name="url">Source Url.</param>
    /// <returns>An object with text and links to other attached resources.</returns>
    public Task<IProxyContent?> TryGetByUrlAsync(string url);
}