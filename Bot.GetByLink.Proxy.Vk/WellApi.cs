using Bot.GetByLink.Common.Infrastructure.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Vk.Regex;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk;

/// <summary>
///     The class of work with the wall mathematicians VK.
/// </summary>
public sealed class WellApi
{
    private readonly VkApi api;
    private readonly ILogger logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WellApi" /> class.
    /// </summary>
    /// <param name="vkApi">API for interaction with VK.</param>
    /// <param name="logger">Interface for logging.</param>
    public WellApi(VkApi vkApi, ILogger<WellApi> logger)
    {
        ArgumentNullException.ThrowIfNull(vkApi);
        ArgumentNullException.ThrowIfNull(logger);

        api = vkApi;
        this.logger = logger;

        WallRegex = new WallRegexWrapper();
        WallPostIdRegex = new WallPostIdRegexWrapper();
    }

    /// <summary>
    ///     Gets regular expression for Wall VK.
    /// </summary>
    public IRegexWrapper WallRegex { get; }

    /// <summary>
    ///     Gets regular expression for Post Id by Wall VK.
    /// </summary>
    public IRegexWrapper WallPostIdRegex { get; }

    /// <summary>
    ///     Get content by post id.
    /// </summary>
    /// <param name="id">Post id.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public async Task<IProxyContent?> TryGetByIdAsync(string? id)
    {
        if (id is null || !WallPostIdRegex.IsMatch(id))
        {
            var text =
                "Id стены не правильный или не подходит\n" +
                "Wall id is not correct or not suitable";
            var emptyContent = new ProxyResponseContent(text);
            return emptyContent;
        }

        try
        {
            var wallId = WallPostIdRegex.Match(id)?.Value;
            if (wallId is null) return null;

            var data = await api.Wall.GetByIdAsync(new List<string> { wallId });
            var (message, urlPicture, urlVideo) = await GetContentAsync(data);

            var content = new ProxyResponseContent(message ?? string.Empty, urlPicture, urlVideo);

            return content;
        }
        catch (Exception ex)
        {
            var message = "ProxyVK : WellApi";
            logger.LogError(ex, message);
            return null;
        }
    }

    private static async Task<(string? Message, IEnumerable<IMediaInfo>? UrlPicture, IEnumerable<IMediaInfo>? UrlVideo)>
        GetContentAsync(WallGetObject? obect)
    {
        if (obect is null) return (null, null, null);

        // TODO: сделать обработку видео?
        var urlPicture = new List<MediaInfo>();

        var post = obect.WallPosts?.FirstOrDefault();
        if (post is null) return (null, null, null);

        foreach (var item in post.Attachments)
            if (item.Instance is Photo photo)
            {
                var maxSize = photo.Sizes.Aggregate((a, b) => a.Height + a.Width > b.Height + b.Width ? a : b);
                var size = await ProxyHelper.GetSizeContentUrlAsync(maxSize.Url.AbsoluteUri);
                urlPicture.Add(new MediaInfo(maxSize.Url.AbsoluteUri, size, MediaType.Photo));
            }

        return (post.Text, urlPicture, null);
    }
}