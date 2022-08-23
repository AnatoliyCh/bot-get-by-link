using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
/// Provides an API for VK Albums.
/// </summary>
public sealed class AlbumStrategy : ContentReturnStrategy
{
    private readonly IContentReturnStrategy photoStrategy;
    private readonly ulong step = 50; //TODO: в конфиг

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="photoStrategy">Provides Api for VK photos.</param>
    public AlbumStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<AlbumStrategy> logger, IContentReturnStrategy photoStrategy)
        : base(api, idResourceRegexWrapper, logger)
    {
        this.photoStrategy = photoStrategy ?? throw new ArgumentNullException(nameof(photoStrategy));
    }

    /// <summary>
    ///     Gets regular expression for Album.
    /// </summary>
    public static IRegexWrapper Regex { get; } = new UrlAlbumRegexWrapper();

    /// <summary>
    ///     Gets a Album by url.
    /// </summary>
    /// <param name="url">Url Album.</param>
    /// <returns>An object with a caption and a link to the photo collection.</returns>
    public override async Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var albumUrl = Regex.Match(url)?.Value;
            var fullId = IdResourceRegexWrapper.Match(albumUrl)?.Value;
            if (fullId is null) return null;

            var (ownerId, albumId) = GetAlbumIds(fullId);
            if (ownerId is null || albumId is null) return null;

            var info = await GetInfobyAlbum((long)ownerId, (long)albumId);
            var media = await GetAlbum((long)ownerId, (long)albumId);

            return new ProxyResponseContent(info, UrlPicture: media);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : AlbumStrategy : TryGetByUrlAsync");
            return null;
        }
    }

    /// <summary>
    /// Return content from a photo collection.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Photo collection.</param>
    /// <returns>A collection of information objects about attached Photos.</returns>
    public override async Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        if (collection is not IEnumerable<Album> albums || !collection.Any()) return null;
        try
        {
            IEnumerable<IMediaInfo> medias = new List<MediaInfo>((int)step);
            var mediasAsList = medias.ToList();
            foreach (var album in albums)
            {
                if (album.OwnerId is null || album.Thumb.AlbumId is null) continue;

                var photos = await GetAlbum((long)album.OwnerId, (long)album.Thumb.AlbumId);
                if (photos is not null && photos.Any()) mediasAsList.AddRange(photos);
            }

            return mediasAsList.Where(item => item is not null);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Proxy VK : AlbumStrategy : TryGetByCollectionAsync");
            return null;
        }
    }

    private static (long? OwnerId, long? AlbumId) GetAlbumIds(string fullId)
    {
        var ids = fullId.Split("_"); // [ownerId, albumId]
        if (long.TryParse(ids[0], out var ownerId) &&
            long.TryParse(ids[1], out var albumId))
            return (ownerId, albumId);

        return (null, null);
    }

    private async Task<string> GetInfobyAlbum(long ownerId, long albumId)
    {
        var photoGetAlbumsParams = new PhotoGetAlbumsParams()
        {
            OwnerId = ownerId,
            AlbumIds = new List<long>() { albumId }
        };
        var album = (await Api.Photo.GetAlbumsAsync(photoGetAlbumsParams)).FirstOrDefault();
        if (album is null) return string.Empty;

        return $"{album.Title}\n{album.Description}";
    }

    private async Task<IEnumerable<IMediaInfo>?> GetAlbum(long ownerId, long albumId)
    {
        var photos = new List<Photo>((int)step);
        var offset = 0ul;
        var photoGetParams = new PhotoGetParams()
        {
            OwnerId = ownerId,
            AlbumId = PhotoAlbumType.Id(albumId),
            Reversed = true,
            Count = step,
            Offset = offset,
        };
        do
        {
            var album = await Api.Photo.GetAsync(photoGetParams);
            if (album is null || album.Count == 0) break;

            photoGetParams.Offset += step;
            photos.AddRange(album);
        }
        while (true);

        return await photoStrategy.TryGetByCollectionAsync(photos);
    }
}
