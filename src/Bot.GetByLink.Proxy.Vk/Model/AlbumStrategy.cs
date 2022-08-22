using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
/// Provides an API for VK albums.
/// </summary>
public sealed class AlbumStrategy : IContentReturnStrategy
{
    private readonly VkApi api;
    private readonly IRegexWrapper idResourceRegexWrapper;
    private readonly ILogger logger;
    private readonly IContentReturnStrategy photoStrategy;
    private readonly ulong step = 50; //TODO: в конфиг

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumStrategy"/> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="photoStrategy">Provides api for VK photos.</param>
    public AlbumStrategy(VkApi api, IRegexWrapper idResourceRegexWrapper, ILogger<AlbumStrategy> logger, IContentReturnStrategy photoStrategy)
    {
        this.api = api ?? throw new ArgumentNullException(nameof(api));
        this.idResourceRegexWrapper = idResourceRegexWrapper ?? throw new ArgumentNullException(nameof(idResourceRegexWrapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.photoStrategy = photoStrategy ?? throw new ArgumentNullException(nameof(photoStrategy));
    }

    /// <summary>
    ///     Gets regular expression for Album.
    /// </summary>
    public static IRegexWrapper AlbumRegex { get; } = new UrlAlbumRegexWrapper();

    /// <summary>
    ///     Gets a Album by url.
    /// </summary>
    /// <param name="url">Url Album.</param>
    /// <returns>An object with a caption and a link to the photo collection.</returns>
    public async Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        try
        {
            var albumUrl = AlbumRegex.Match(url)?.Value;
            var fullId = idResourceRegexWrapper.Match(albumUrl)?.Value;
            if (fullId is null) return null;

            var (ownerId, albumId) = GetAlbumIds(fullId);
            if (ownerId == null || albumId == null) return null;

            var photos = new List<Photo>((int)step);
            var offset = 0ul;
            var photoGetParams = new PhotoGetParams()
            {
                OwnerId = ownerId,
                AlbumId = PhotoAlbumType.Id((long)albumId),
                Reversed = true,
                Count = step,
                Offset = offset,
            };
            var info = await GetInfobyAlbum((long)ownerId, (long)albumId);

            do
            {
                var album = await api.Photo.GetAsync(photoGetParams);
                if (album is null || album.Count == 0) break;

                photoGetParams.Offset += step;
                photos.AddRange(album);
            }
            while (true);
            var media = await photoStrategy.TryGetByCollectionAsync(photos);

            return new ProxyResponseContent(info, UrlPicture: media);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Proxy VK : AlbumStrategy : TryGetByUrlAsync");
            return null;
        }
    }


    public Task<IEnumerable<IMediaInfo>> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
        where T : MediaAttachment
    {
        throw new NotImplementedException();
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
        var album = (await api.Photo.GetAlbumsAsync(photoGetAlbumsParams)).FirstOrDefault();
        if (album is null) return string.Empty;

        return $"{album.Title}\n{album.Description}";
    }
}
