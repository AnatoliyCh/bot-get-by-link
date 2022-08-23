using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Abstractions;

/// <summary>
/// The base class of content return strategies.
/// </summary>
public abstract class ContentReturnStrategy : IContentReturnStrategy
{
    /// <summary>
    /// Gets API for interaction with VK.
    /// </summary>
    protected VkApi Api { get; }

    /// <summary>
    /// Gets regular expression for VK resource Id.
    /// </summary>
    protected IRegexWrapper IdResourceRegexWrapper { get; }

    /// <summary>
    /// Gets interface for logging.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentReturnStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    protected ContentReturnStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger logger)
    {
        Api = api ?? throw new ArgumentNullException(nameof(api));
        IdResourceRegexWrapper = idResourceRegexWrapper ?? throw new ArgumentNullException(nameof(idResourceRegexWrapper));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Gets a resource by url.
    /// </summary>
    /// <param name="url">Url resource.</param>
    /// <returns>An object with a caption and a link to the resource.</returns>
    public abstract Task<IProxyContent?> TryGetByUrlAsync(string url);

    /// <summary>
    /// Return content from a resource collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Resource collection.</param>
    /// <returns>A collection of information objects about attached resources.</returns>
    public abstract Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
        where T : MediaAttachment;
}