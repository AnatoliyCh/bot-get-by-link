using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Model.Strategies;

/// <summary>
///     Provides Api for VK Videos.
///     No access.
/// </summary>
public sealed class VideoStrategy : ContentReturnStrategy
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VideoStrategy" /> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public VideoStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<VideoStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
    }

    /// <summary>
    ///     Gets regular expression for Video.
    /// </summary>
    public static IRegexWrapper Regex { get; } = new UrlVideoRegexWrapper();

    /// <summary>
    ///     Gets a video by url.
    /// </summary>
    /// <param name="url">Url video.</param>
    /// <returns>An object with a caption and a link to the video.</returns>
    public override Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        var nullTask = Task.FromResult<IProxyContent?>(null);
        try
        {
            var videoUrl = Regex.Match(url)?.Value;
            var id = IdResourceRegexWrapper.Match(videoUrl)?.Value;
            if (id is null) return nullTask;

            return Task.FromResult<IProxyContent?>(new ProxyResponseContent("/help"));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : VideoStrategy : TryGetByUrlAsync");
            return nullTask;
        }
    }

    /// <summary>
    ///     Return content from a Videos collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Video collection.</param>
    /// <returns>A collection of information objects about attached Videos.</returns>
    public override Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        var nullTask = Task.FromResult<IEnumerable<IMediaInfo>?>(null);
        if (collection is not IEnumerable<Video> videos || !collection.Any()) return nullTask;
        try
        {
            var medias = new List<MediaInfoExtra>(videos.Count());
            foreach (var item in videos)
            {
                if (item is null) continue;

                var url = $"https://vk.com/video{item.OwnerId}_{item.Id}";
                medias.Add(new MediaInfoExtra(url, -1, MediaType.Video, item.Width, item.Height, item.Title));
            }

            return Task.FromResult<IEnumerable<IMediaInfo>?>(medias);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : VideoStrategy : TryGetByCollectionAsync");
            return nullTask;
        }
    }
}