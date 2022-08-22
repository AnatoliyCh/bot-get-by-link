using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
///     Provides api for VK photos.
///     Not albums!.
/// </summary>
public sealed class PhotoStrategy : IContentReturnStrategy
{
    private readonly VkApi api;
    private readonly IRegexWrapper idResourceRegexWrapper;
    private readonly ILogger logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PhotoStrategy" /> class.
    /// </summary>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="api">API for interaction with VK.</param>
    public PhotoStrategy(IRegexWrapper idResourceRegexWrapper, ILogger logger, VkApi api)
    {
        this.idResourceRegexWrapper =
            idResourceRegexWrapper ?? throw new ArgumentNullException(nameof(idResourceRegexWrapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.api = api ?? throw new ArgumentNullException(nameof(api));
    }

    /// <summary>
    ///     Gets regular expression for photos.
    /// </summary>
    public static IRegexWrapper PhotoRegex { get; } = new UrlPhotoRegexWrapper();

    /// <summary>
    ///     Gets a photo by url.
    /// </summary>
    /// <param name="url">Url photo.</param>
    /// <returns>An object with a caption and a link to the photo.</returns>
    public async Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var photoUrl = PhotoRegex.Match(url)?.Value;
            var photoId = idResourceRegexWrapper.Match(photoUrl)?.Value;
            if (photoId is null) return null;

            var photo = (await api.Photo.GetByIdAsync(new[] { photoId })).FirstOrDefault();
            if (photo is null) return null;

            var maxSize = photo.Sizes.Aggregate((a, b) => a.Height + a.Width > b.Height + b.Width ? a : b);
            var size = await ProxyHelper.GetSizeContentUrlAsync(maxSize.Url.AbsoluteUri);
            var mediaInfo = new MediaInfo(maxSize.Url.AbsoluteUri, size, MediaType.Photo);

            return new ProxyResponseContent(photo.Text, UrlPicture: new[] { mediaInfo });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Proxy VK: PhotoStrategy");
            return null;
        }
    }
}