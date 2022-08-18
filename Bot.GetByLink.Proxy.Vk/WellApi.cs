using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Vk.Regexs;
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
            if (message is null && urlPicture is null && urlVideo is null) return null;

            return new ProxyResponseContent(message ?? string.Empty, urlPicture, urlVideo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ProxyVK : WellApi");
            return null;
        }
    }

    private static async Task<(string? Message, IEnumerable<IMediaInfo>? UrlPicture, IEnumerable<IMediaInfo>? UrlVideo)>
        GetContentAsync(WallGetObject? obect)
    {
        if (obect is null) return (null, null, null);

        var post = obect.WallPosts?.FirstOrDefault();
        if (post is null) return (null, null, null);

        // TODO: сделать обработку видео?
        var capacity = post.Attachments.Count;
        var urlPicture = new MediaInfo[capacity];
        var tasks = new Task[capacity];

        for (var i = 0; i < capacity; i++)
            if (post.Attachments[i].Instance is Photo photo)
            {
                var position = i; // i is needed to arrange the artifacts in order.
                tasks[position] = Task.Run(async () =>
                {
                    var maxSize = photo.Sizes.Aggregate((a, b) => a.Height + a.Width > b.Height + b.Width ? a : b);
                    var size = await ProxyHelper.GetSizeContentUrlAsync(maxSize.Url.AbsoluteUri);
                    urlPicture[position] = new MediaInfo(maxSize.Url.AbsoluteUri, size, MediaType.Photo);
                });
            }
            else
            {
                tasks[i] = Task.CompletedTask;
            }

        if (tasks.Length > 0) await Task.WhenAll(tasks);

        urlPicture = urlPicture.Where(item => item is not null).ToArray();

        return (post.Text, urlPicture.Length > 0 ? urlPicture : null, null);
    }
}