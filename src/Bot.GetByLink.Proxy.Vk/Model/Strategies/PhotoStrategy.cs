using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
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
///     Provides Api for VK Photos.
///     Not albums!.
/// </summary>
public sealed class PhotoStrategy : ContentReturnStrategy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    public PhotoStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<PhotoStrategy> logger)
        : base(api, idResourceRegexWrapper, logger)
    {
    }

    /// <summary>
    ///     Gets regular expression for Photo.
    /// </summary>
    public static IRegexWrapper Regex { get; } = new UrlPhotoRegexWrapper();

    /// <summary>
    ///     Gets a photo by url.
    /// </summary>
    /// <param name="url">Url photo.</param>
    /// <returns>An object with a caption and a link to the photo.</returns>
    public override async Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var photoUrl = Regex.Match(url)?.Value;
            var photoId = IdResourceRegexWrapper.Match(photoUrl)?.Value;
            if (photoId is null) return null;

            var photo = (await Api.Photo.GetByIdAsync(new[] { photoId })).FirstOrDefault();
            if (photo is null) return null;

            var maxSize = photo.Sizes.Aggregate((a, b) => a.Height + a.Width > b.Height + b.Width ? a : b);
            var size = await ProxyHelper.GetSizeContentUrlAsync(maxSize.Url.AbsoluteUri);
            var mediaInfo = new MediaInfo(maxSize.Url.AbsoluteUri, size, MediaType.Photo);

            return new ProxyResponseContent(photo.Text, UrlPicture: new[] { mediaInfo });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : PhotoStrategy : TryGetByUrlAsync");
            return null;
        }
    }

    /// <summary>
    /// Return content from a Photo collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Photo collection.</param>
    /// <returns>A collection of information objects about attached Photos.</returns>
    public override async Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        if (collection is not IEnumerable<Photo> photos || !collection.Any()) return null;
        try
        {
            var capacity = photos.Count();
            var photoUrls = new MediaInfo[capacity];
            var tasks = new Task[capacity];
            var index = 0;
            foreach (var photo in photos)
            {
                var position = index; // i is needed to arrange the photos in order.
                index++;
                if (photo is null)
                {
                    tasks[position] = Task.CompletedTask;
                    continue;
                }

                tasks[position] = Task.Run(async () =>
                {
                    var maxSize = photo.Sizes.Aggregate((a, b) => a.Height + a.Width > b.Height + b.Width ? a : b);
                    var size = await ProxyHelper.GetSizeContentUrlAsync(maxSize.Url.AbsoluteUri);
                    photoUrls[position] = new MediaInfo(maxSize.Url.AbsoluteUri, size, MediaType.Photo);
                });
            }

            if (tasks.Length > 0) await Task.WhenAll(tasks);

            return photoUrls.Where(item => item is not null);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : PhotoStrategy : TryGetByCollectionAsync");
            return null;
        }
    }
}
