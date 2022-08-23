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
/// Provides Api for VK Videos.
/// No access.
/// </summary>
public sealed class VideoStrategy : ContentReturnStrategy
{
    private readonly StringBuilder builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public VideoStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<VideoStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
        builder = new StringBuilder();
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
        try
        {
            var videoUrl = Regex.Match(url)?.Value;
            var videoId = IdResourceRegexWrapper.Match(videoUrl)?.Value;
            if (videoId is null) return Task.FromResult<IProxyContent?>(null);

            builder
                .AppendFormat("https://vk.com/video{0}", videoId)
                .AppendLine()
                .AppendLine("/help");
            var content = new ProxyResponseContent(builder.ToString());
            builder.Clear();

            return Task.FromResult<IProxyContent?>(content);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : VideoStrategy : TryGetByUrlAsync");
            return Task.FromResult<IProxyContent?>(null);
        }
    }

    /// <summary>
    /// Return content from a Videos collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Video collection.</param>
    /// <returns>A collection of information objects about attached Videos.</returns>
    public override Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        throw new NotImplementedException();
    }
}
