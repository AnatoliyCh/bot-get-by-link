using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using System.Text;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
/// Provides Api for VK Docs.
/// No access.
/// </summary>
public sealed class DocStrategy : ContentReturnStrategy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public DocStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<DocStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
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
        var nullTask = Task.FromResult<IProxyContent?>(null);
        try
        {
            var docUrl = Regex.Match(url)?.Value;
            var docId = IdResourceRegexWrapper.Match(docUrl)?.Value;
            if (docId is null) return nullTask;

            var builder =
                new StringBuilder($"https://vk.com/doc{docId}")
                .AppendLine()
                .AppendLine("/help");

            return Task.FromResult<IProxyContent?>(new ProxyResponseContent(builder.ToString()));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : DocStrategy : TryGetByUrlAsync");
            return nullTask;
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
        var nullTask = Task.FromResult<IEnumerable<IMediaInfo>?>(null);
        if (collection is not IEnumerable<Document> docs || !collection.Any()) return nullTask;
        try
        {
            var medias = new List<MediaInfoExtra>(docs.Count());
            foreach (var item in docs)
            {
                if (item is null) continue;

                var url = $"https://vk.com/doc{item.OwnerId}_{item.Id}";
                medias.Add(new MediaInfoExtra(url, -1, MediaType.Document, item.Title, string.Empty));
            }

            return Task.FromResult<IEnumerable<IMediaInfo>?>(medias);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : DocStrategy : TryGetByCollectionAsync");
            return nullTask;
        }
    }
}
