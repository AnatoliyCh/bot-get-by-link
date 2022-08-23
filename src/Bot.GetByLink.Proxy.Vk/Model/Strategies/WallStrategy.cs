using System.Text;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Abstractions;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model.Attachments;

namespace Bot.GetByLink.Proxy.Vk.Model.Strategies;

/// <summary>
///     Provides an API for VK Wall.
/// </summary>
public sealed class WallStrategy : ContentReturnStrategy
{
    private readonly IContentReturnStrategy albumStrategy;
    private readonly IContentReturnStrategy docStrategy;
    private readonly IContentReturnStrategy photoStrategy;
    private readonly IContentReturnStrategy videoStrategy;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WallStrategy" /> class.
    /// </summary>
    /// <param name="api">API for interaction with VK.</param>
    /// <param name="idResourceRegexWrapper"> Regular expression for VK resource Id.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="photoStrategy">Provides Api for VK Photos.</param>
    /// <param name="albumStrategy">Provides an API for VK Albums.</param>
    /// <param name="docStrategy">Provides Api for VK Docs.</param>
    /// <param name="videoStrategy">Provides Api for VK Videos.</param>
    public WallStrategy(
        VkApi api,
        IRegexWrapper idResourceRegexWrapper,
        ILogger<WallStrategy> logger,
        IContentReturnStrategy photoStrategy,
        IContentReturnStrategy albumStrategy,
        IContentReturnStrategy docStrategy,
        IContentReturnStrategy videoStrategy)
        : base(api, idResourceRegexWrapper, logger)
    {
        this.photoStrategy = photoStrategy ?? throw new ArgumentNullException(nameof(photoStrategy));
        this.albumStrategy = albumStrategy ?? throw new ArgumentNullException(nameof(albumStrategy));
        this.docStrategy = docStrategy ?? throw new ArgumentNullException(nameof(docStrategy));
        this.videoStrategy = videoStrategy ?? throw new ArgumentNullException(nameof(videoStrategy));
    }

    /// <summary>
    ///     Gets regular expression for Wall.
    /// </summary>
    public static IRegexWrapper Regex { get; } = new UrlWallRegexWrapper();

    /// <summary>
    ///     Gets a post by url.
    /// </summary>
    /// <param name="url">Url post.</param>
    /// <returns>An object with a caption and a link to the post.</returns>
    public override async Task<IProxyContent?> TryGetByUrlAsync(string url)
    {
        var wallUrl = Regex.Match(url)?.Value;
        var wallId = IdResourceRegexWrapper.Match(wallUrl)?.Value;
        if (wallId is null) return null;

        var post = (await Api.Wall.GetByIdAsync(new[] { wallId })).WallPosts.FirstOrDefault();
        var repost = post?.CopyHistory.FirstOrDefault();
        if (post is null) return null;

        var proxyResponseContent = new ProxyResponseContent[2];
        var tasks = new List<Task>
        {
            Task.Run(async () =>
            {
                var content = await GetContentByPost(post);
                if (content is not null) proxyResponseContent[0] = (ProxyResponseContent)content;
            }),
            Task.Run(async () =>
            {
                if (repost is null) return;

                var content = await GetContentByPost(repost);
                if (content is not null) proxyResponseContent[1] = (ProxyResponseContent)content;
            })
        };

        if (tasks.Count > 0) await Task.WhenAll(tasks);
        if (!proxyResponseContent.Any(item => item is not null)) return null;

        var builder = new StringBuilder();
        IEnumerable<IMediaInfo> pictures = new List<IMediaInfo>();
        foreach (var content in proxyResponseContent)
        {
            if (content is null) continue;

            builder.AppendLine(content.Text);
            if (content.UrlPicture is not null) pictures = pictures.Concat(content.UrlPicture);
        }

        return new ProxyResponseContent(builder.ToString(), UrlPicture: pictures);
    }

    /// <summary>
    ///     Return content from a Wall collection.
    ///     !! Not implemented.
    /// </summary>
    /// <typeparam name="T">Collection Item Type.</typeparam>
    /// <param name="collection">Wall collection.</param>
    /// <returns>A collection of information objects about attached Walls.</returns>
    /// <exception cref="NotImplementedException">Not implemented.</exception>
    public override Task<IEnumerable<IMediaInfo>?> TryGetByCollectionAsync<T>(IEnumerable<T> collection)
    {
        throw new NotImplementedException();
    }

    private async Task<IProxyContent?> GetContentByPost(Post post)
    {
        var photos = new List<Photo>();
        var albums = new List<Album>();
        var documents = new List<Document>();
        var videos = new List<Video>();
        var builder = new StringBuilder();

        foreach (var item in post.Attachments)
        {
            if (item is null) continue;

            switch (item.Instance)
            {
                case Photo photo:
                    photos.Add(photo);
                    break;
                case Album album:
                    albums.Add(album);
                    break;
                case Document doc:
                    documents.Add(doc);
                    break;
                case Video video:
                    videos.Add(video);
                    break;
            }
        }

        builder.AppendLine(post.Text ?? string.Empty);
        var mediaPhotos = await photoStrategy.TryGetByCollectionAsync(photos);
        var mediaAlbums = await albumStrategy.TryGetByCollectionAsync(albums);
        var mediaDocs = await docStrategy.TryGetByCollectionAsync(documents);
        var mediaVideos = await videoStrategy.TryGetByCollectionAsync(videos);

        // pictures
        IEnumerable<IMediaInfo> pictures = new List<IMediaInfo>();
        if (mediaPhotos is not null) pictures = pictures.Concat(mediaPhotos);
        if (mediaAlbums is not null) pictures = pictures.Concat(mediaAlbums);

        // docs
        if (mediaDocs is not null && mediaDocs.Any())
            foreach (var item in mediaDocs)
            {
                if (item is not MediaInfoExtra doc) continue;

                builder
                    .AppendLine()
                    .AppendLine(doc.Title)
                    .AppendLine(doc.Url);
            }

        if (mediaVideos is not null && mediaVideos.Any())
            foreach (var item in mediaVideos)
            {
                if (item is not MediaInfoExtra video) continue;

                builder
                    .AppendLine()
                    .AppendLine(video.Title)
                    .AppendLine(video.Url);
            }

        return new ProxyResponseContent(builder.ToString(), UrlPicture: pictures);
    }
}