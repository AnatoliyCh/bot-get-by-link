using Bot.GetByLink.Common.Interfaces.Proxy;
using VkNet.Model.Attachments;

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

    /// <summary>
    /// Returning Content from the Attachment Collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Collection of elements.</param>
    /// <returns>A collection of information objects about attachments.</returns>
    public Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
        where T : MediaAttachment;
}