using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using System.Text;
using VkNet;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
/// Provides Api for VK Docs.
/// No access.
/// </summary>
public sealed class DocStrategy : ContentReturnStrategy
{
    private readonly StringBuilder builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public DocStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<DocStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
        builder = new StringBuilder();
    }

    /// <summary>
    ///     Gets regular expression for Document.
    /// </summary>
    public static IRegexWrapper Regex { get; } = new UrlDocRegexWrapper();

    /// <summary>
    ///     Gets a url doc by url.
    /// </summary>
    /// <param name="url">Url doc.</param>
    /// <returns>An object with a caption and a link to the doc.</returns>
    public override Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var docUrl = Regex.Match(url)?.Value;
            var docId = IdResourceRegexWrapper.Match(docUrl)?.Value;
            if (docId is null) return Task.FromResult<IProxyContent?>(null);

            builder
                .AppendFormat("https://vk.com/doc{0}", docId)
                .AppendLine()
                .AppendLine("/help");
            var content = new ProxyResponseContent(builder.ToString());
            builder.Clear();

            return Task.FromResult<IProxyContent?>(content);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : DocStrategy : TryGetByUrlAsync");
            return Task.FromResult<IProxyContent?>(null);
        }
    }

    /// <summary>
    /// Return content from a doc collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Doc collection.</param>
    /// <returns>A collection of information objects about attached Docs.</returns>
    public override Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        throw new NotImplementedException();
    }
}
