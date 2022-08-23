using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using System.Text;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
/// Provides api for VK Docs.
/// No access.
/// </summary>
public sealed class DocStrategy : IContentReturnStrategy
{
    private readonly VkApi api;
    private readonly IRegexWrapper idResourceRegexWrapper;
    private readonly ILogger logger;
    private readonly StringBuilder builder;

    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="DocStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public DocStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<DocStrategy> logger)
    {
        this.api = api ?? throw new ArgumentNullException(nameof(api));
        this.idResourceRegexWrapper = idResourceRegexWrapper ?? throw new ArgumentNullException(nameof(idResourceRegexWrapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        builder = new();
    }

    /// <summary>
    ///     Gets regular expression for Document.
    /// </summary>
    public static IRegexWrapper DocRegex { get; } = new UrlDocRegexWrapper();

    /// <summary>
    ///     Gets a url doc by url.
    /// </summary>
    /// <param name="url">Url doc.</param>
    /// <returns>An object with a caption and a link to the doc.</returns>
    public Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var docUrl = DocRegex.Match(url)?.Value;
            var docId = idResourceRegexWrapper.Match(docUrl)?.Value;
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
            logger.LogError(ex, "Proxy VK : AlbumStrategy : DocStrategy");
            return Task.FromResult<IProxyContent?>(null);
        }
    }

    public Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection) where T : MediaAttachment
    {
        throw new NotImplementedException();
    }
}
