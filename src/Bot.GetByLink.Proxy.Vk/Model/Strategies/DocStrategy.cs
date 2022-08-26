using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Infrastructure.Regexs;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Model.Strategies;

/// <summary>
///     Provides Api for VK Docs.
///     No access.
/// </summary>
public sealed class DocStrategy : ContentReturnStrategy
{
    private readonly IRegexWrapper gifRegexWrapper;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DocStrategy" /> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public DocStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<DocStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
        gifRegexWrapper = new GifRegexWrapper();
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
            var id = IdResourceRegexWrapper.Match(docUrl)?.Value;
            if (id is null) return nullTask;

            return Task.FromResult<IProxyContent?>(new ProxyResponseContent("/help"));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : DocStrategy : TryGetByUrlAsync");
            return nullTask;
        }
    }

    /// <summary>
    ///     Return content from a doc collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Doc collection.</param>
    /// <returns>A collection of information objects about attached Docs.</returns>
    public override async Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        if (collection is not IEnumerable<Document> docs || !collection.Any()) return null;
        try
        {
            var medias = new List<MediaInfoExtra>(docs.Count());
            foreach (var item in docs)
            {
                if (item is null) continue;
                if (IsGif(item))
                {
                    var size = await ProxyHelper.GetSizeContentUrlAsync(item.Uri);
                    medias.Add(new MediaInfoExtra(item.Uri, size, MediaType.Gif, IsArtifact: true));
                    continue;
                }

                medias.Add(new MediaInfoExtra(item.Uri, -1, MediaType.Document, item.Title, IsArtifact: false));
            }

            return medias;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : DocStrategy : TryGetByCollectionAsync");
            return null;
        }
    }

    private bool IsGif(Document doc)
    {
        return doc.Ext == "gif" && gifRegexWrapper.IsMatch(doc.Title, RegexOptions.IgnoreCase);
    }
}