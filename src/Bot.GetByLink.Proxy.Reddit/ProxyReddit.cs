using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Abstractions.Proxy;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Infrastructure.Regexs;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Reddit.Model.GfyCat;
using Bot.GetByLink.Proxy.Reddit.Model.Imgur;
using Bot.GetByLink.Proxy.Reddit.Model.Reddit;
using Bot.GetByLink.Proxy.Reddit.Model.RedGif;
using Bot.GetByLink.Proxy.Reddit.Regexs;
using Microsoft.Extensions.Logging;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
///     Reddit API for getting post content by id or url.
/// </summary>
public sealed class ProxyReddit : ProxyService
{
    private readonly string appId;
    private readonly IRegexWrapper galleryRegex;
    private readonly IRegexWrapper gifRegex;
    private readonly IRegexWrapper gifvRegex;
    private readonly IRegexWrapper picturesRegex;
    private readonly IRegexWrapper gfycatRegex;
    private readonly IRegexWrapper redGifRegex;
    private readonly IRegexWrapper imgurRegex;
    private readonly string secretId;
    private readonly string ImgurAppId;
    private readonly string urlBase = "www.reddit.com";
    private readonly ILogger logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyReddit" /> class.
    /// </summary>
    /// <param name="configuration">Bot configuration.</param>
    /// <param name="logger">Interface for logging ProxyReddit.</param>
    public ProxyReddit(IBotConfiguration? configuration, ILogger<ProxyReddit> logger)
        : base(new[] { new RedditPostRegexWrapper() })
    {
        ArgumentNullException.ThrowIfNull(configuration);

        appId = configuration.Proxy.Reddit.AppId ?? string.Empty;
        secretId = configuration.Proxy.Reddit.Secret ?? string.Empty;
        ImgurAppId = configuration.Proxy.Reddit.SubServices.Imgur.AppId ?? string.Empty;
        picturesRegex = new PictureRegexWrapper();
        gifRegex = new GifRegexWrapper();
        galleryRegex = new RedditGalleryRegexWrapper();
        gfycatRegex = new GfyCatRegexWrapper();
        redGifRegex = new RedGifRegexWrapper();
        imgurRegex = new ImgurRegexWrapper();
        gifvRegex = new GifvRegexWrapper();
        this.logger = logger;
    }

    /// <summary>
    ///     Method for getting reddit post content by post url.
    /// </summary>
    /// <param name="url">Url to a reddit post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override async Task<IProxyContent?> GetContentUrlAsync(string url)
    {
        var postId = galleryRegex.IsMatch(url) ? GetRedditId(url, "gallery/") : GetRedditId(url, "comments/");
        if (postId is null) return null;
        return await TryGetContentIdAsync(postId);
    }

    /// <summary>
    ///     Method for getting reddit post content by post id.
    /// </summary>
    /// <param name="postId">Post ID.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    /// <exception cref="ArgumentNullException">Returned if an empty postId was passed.</exception>
    public async Task<IProxyContent?> TryGetContentIdAsync(string postId)
    {
        try
        {
            await GetAccessTokenAsync();
            var fullPostInfo = await GetFullPostInfo($"t3_{postId}");
            var postData = GetPostData(fullPostInfo);
            var crossPostId = GetParentPostIdAsync(postData);
            long size;
            if (!string.IsNullOrWhiteSpace(crossPostId) && postData?.СrosspostParentList is not null && postData.СrosspostParentList.Count > 0)
            {
                fullPostInfo = await GetFullPostInfo($"{crossPostId}");
                postData = GetPostData(fullPostInfo);
            }

            if (postData is null) return null;

            var header = postData.Title ?? string.Empty;

            if (!postData.IsRedditMediaDomain)
            {
                var (error, postContent) = postData.Url switch
                {
                    var gfycat when gfycatRegex.IsMatch(gfycat, RegexOptions.IgnoreCase) => await GetContentGfyCat(gfycat, header, postData.Over18),
                    var redGif when redGifRegex.IsMatch(redGif, RegexOptions.IgnoreCase) => await GetContentRedGif(redGif, header),
                    var imgur when imgurRegex.IsMatch(imgur, RegexOptions.IgnoreCase) => await GetContentImgur(postData, header),
                    _ => ("not match", null),
                };
                if (postContent is not null) return postContent;
                if (postData.Preview?.RedditVideoPreview is not null && error == "not found")
                {
                    var video = postData.Preview.RedditVideoPreview;
                    size = await ProxyHelper.GetSizeContentUrlAsync(video.FallbackUrl);
                    return new ProxyResponseContent(postData.Url, header, UrlVideo:
                        new[] { new MediaInfo(video.FallbackUrl, size, MediaType.Video, video.Width, video.Height) });
                }
            }

            if (postData.GalleryData is not null)
            {
                var galleryMedia = await GetGalleryMedia(postData);
                return new ProxyResponseContent(string.Empty, header,
                    galleryMedia);
            }

            if (postData.Media == null && !postData.IsVideo &&
                (string.IsNullOrWhiteSpace(postData.Url) && !string.IsNullOrWhiteSpace(postData.SelfText)))
                return new ProxyResponseContent(postData.SelfText, header);

            if (picturesRegex.IsMatch(postData.Url, RegexOptions.IgnoreCase))
            {
                var source = postData.Preview?.Images?.FirstOrDefault()?.Source;
                if (source is null || string.IsNullOrWhiteSpace(source.Url))
                {
                    size = await ProxyHelper.GetSizeContentUrlAsync(postData.Url);
                    return new ProxyResponseContent(string.Empty, header,
                        new[] { new MediaInfo(postData.Url, size, MediaType.Photo) });
                }

                size = await ProxyHelper.GetSizeContentUrlAsync(source.Url);
                return new ProxyResponseContent(string.Empty, header,
                    new[] { new MediaInfo(source.Url, size, MediaType.Photo, source.Width, source.Height) });
            }

            if (gifRegex.IsMatch(postData.Url?.ToLower()))
            {
                var source = postData.Preview?.Images?.FirstOrDefault()?.Source; // trick
                size = await ProxyHelper.GetSizeContentUrlAsync(postData.Url);
                return new ProxyResponseContent(string.Empty, header, UrlVideo:
                    new[] { new MediaInfo(postData.Url, size, MediaType.Video, source.Width, source.Height) });
            }

            if (postData.Media is not null)
            {
                return await HandlerMediaReddit(postData.Media, postData.Url, header);
            }

            return new ProxyResponseContent($"{(string.IsNullOrWhiteSpace(postData.Url) ? string.Empty : $"{postData.Url}\n")}{postData.SelfText}", header);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Proxy Reddit : TryGetContentIdAsync");
            return null;
        }
    }

    /// <summary>
    ///     Function for get reddit id.
    /// </summary>
    /// <param name="url">Url reddit post or gallery.</param>
    /// <param name="findString">gallery/ or comments/.</param>
    /// <returns>Reddit id.</returns>
    private static string GetRedditId(string url, string findString)
    {
        var cutUrlPost = url[(url.IndexOf(findString) + findString.Length)..];
        var indexCutSlash = cutUrlPost.IndexOf("/");
        if (indexCutSlash == -1) return cutUrlPost;
        return cutUrlPost[..indexCutSlash];
    }

    /// <summary>
    ///     Function for get post data.
    /// </summary>
    /// <param name="post">Reddit post.</param>
    /// <returns>Reddit post data.</returns>
    private static RedditPostData? GetPostData(RedditPost? post)
    {
        return post?.Data?.Children?.FirstOrDefault()?.Data;
    }

    /// <summary>
    ///     Function for get parent post.
    /// </summary>
    /// <param name="post">Post in json.</param>
    /// <returns>Id parent post if there is parent post.</returns>
    private static string? GetParentPostIdAsync(RedditPostData? post)
    {
        return post?.CrosspostParent;
    }

    /// <summary>
    ///     Function for get gallery media.
    /// </summary>
    /// <param name="post">Post in json.</param>
    /// <returns>List media info </returns>
    private static async Task<IEnumerable<IMediaInfo>> GetGalleryMedia(RedditPostData? post)
    {
        var galleryData = post?.GalleryData;
        var mediaData = post?.MediaMetadata;
        if (galleryData?.Items is null || mediaData is null) return Enumerable.Empty<IMediaInfo>();
        var tasks = new Task[mediaData.Count];
        var index = 0;
        foreach (var media in mediaData.Select(x => x.Value))
        {
            var position = index; // i is needed to arrange the photos in order.
            index++;
            if (string.IsNullOrWhiteSpace(media?.SourceElements?.Url) &&
                string.IsNullOrWhiteSpace(media?.SourceElements?.Gif))
            {
                tasks[position] = Task.CompletedTask;
                continue;
            }

            tasks[position] = Task.Run(async () =>
            {
                media.SourceElements.Size = await ProxyHelper.GetSizeContentUrlAsync(media.ElementType == "Image"
                    ? media.SourceElements.Url
                    : media.SourceElements.Gif);
            });
        }

        if (tasks.Length > 0) await Task.WhenAll(tasks);
        return mediaData.Select(media =>
            new MediaInfo(
                media.Value.ElementType == "Image" ? media.Value.SourceElements.Url : media.Value.SourceElements.Gif,
                media.Value.SourceElements.Size, media.Value.ElementType == "Image" ? MediaType.Photo : MediaType.Video,
                media.Value.SourceElements.Width, media.Value.SourceElements.Height));
    }

    /// <summary>
    ///     Fucntion for handle media reddit.
    /// </summary>
    /// <param name="redditPostMedia">Media reddit.</param>
    /// <param name="url">Url in source.</param>
    /// <param name="header">Title post reddit.</param>
    /// <returns>Content media reddit.</returns>
    private async Task<IProxyContent?> HandlerMediaReddit(RedditPostMedia redditPostMedia, string url, string header)
    {
        long size;
        if (redditPostMedia.Oembed is null && redditPostMedia.RedditVideo is null) return null;
        if (redditPostMedia.Oembed is not null)
        {
            var video = redditPostMedia.Oembed;
            if (gifRegex.IsMatch(video?.ThumbnailUrl?.ToLower()))
            {
                size = await ProxyHelper.GetSizeContentUrlAsync(video.ThumbnailUrl);
                return new ProxyResponseContent(string.Empty, header, UrlVideo:
                    new[] { new MediaInfo(video.ThumbnailUrl, size, MediaType.Video, video.ThumbnailWidth, video.ThumbnailHeight) });
            }
            else
            {
                return new ProxyResponseContent(url, header);
            }
        }

        var redditVideo = redditPostMedia.RedditVideo;
        if (string.IsNullOrWhiteSpace(redditVideo?.FallbackUrl)) return null;
        size = await ProxyHelper.GetSizeContentUrlAsync(redditVideo.FallbackUrl);
        return new ProxyResponseContent(string.Empty, header, UrlVideo:
            new[] { new MediaInfo(redditVideo.FallbackUrl, size, MediaType.Video, redditVideo.Width, redditVideo.Height) });
    }

    /// <summary>
    ///     Function for get json post.
    /// </summary>
    /// <param name="postId">Id post.</param>
    /// <returns>Post in json.</returns>
    private async Task<RedditPost?> GetFullPostInfo(string postId)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri($"https://{urlBase}")
        };
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/info.json?id={postId}");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var text = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RedditPost>(text);
    }

    /// <summary>
    ///     Function for get accsess token from reddit.
    /// </summary>
    /// <returns>Accsess token reddit.</returns>
    /// <exception cref="HttpRequestException">
    ///     Returned if it was not possible to authorize in reddit or it returned empty
    ///     answer.
    /// </exception>
    private async Task<string> GetAccessTokenAsync()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri($"https://{urlBase}")
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/access_token");

        var byteArray = new UTF8Encoding().GetBytes($"{appId}:{secretId}");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var formData = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("device_id", appId)
        };

        request.Content = new FormUrlEncodedContent(formData);
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var accsessTokenObject =
            JsonSerializer.Deserialize<RedditAccessToken>(await response.Content.ReadAsStringAsync());
        if (accsessTokenObject == null || string.IsNullOrWhiteSpace(accsessTokenObject.AccessToken))
            throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
        return accsessTokenObject.AccessToken;
    }

    /// <summary>
    ///     Function for get content gfycat.com.
    /// </summary>
    /// <param name="url">Url gfycat.</param>
    /// <param name="header">Header reddit post.</param>
    /// <param name="nsfw">Flag nsfw post.</param>
    /// <returns>Content gfycat.</returns>
    private async Task<(string Error, ProxyResponseContent? Content)> GetContentGfyCat(string? url, string header, bool nsfw)
    {
        if (url is null) return ("not valid url", null);
        var id = url[(url.LastIndexOf("/") + 1)..];
        var shiftIndex = id.IndexOf("-");
        if (shiftIndex != -1) id = id[..shiftIndex];

        var client = new HttpClient
        {
            BaseAddress = new Uri($"https://api.gfycat.com/v1/gfycats/{id}")
        };
        var request = new HttpRequestMessage(HttpMethod.Get, string.Empty);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode && nsfw)
            return await GetContentRedGfyId(id, header);

        var text = await response.Content.ReadAsStringAsync();

        var gfycatData = JsonSerializer.Deserialize<GfycatData>(text);
        if (gfycatData is null || gfycatData.GfyItem is null || gfycatData.GfyItem.ContentUrls is null) return ("not found", null);

        if (gfycatData.GfyItem.HasAudio)
        {
            var video = gfycatData.GfyItem.ContentUrls["mobile"];
            if (video is null || string.IsNullOrWhiteSpace(video.Url)) return ("not found", null);
            return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
                new[] { new MediaInfo(video.Url, ProxyHelper.GetMbFromByte(video.Size ?? 0), MediaType.Video, video.Width, video.Height) }));
        }

        var gif = gfycatData.GfyItem.ContentUrls["max5mbGif"];
        if (gif is null || string.IsNullOrWhiteSpace(gif.Url)) return ("not found", null);
        return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
            new[] { new MediaInfo(gif.Url, ProxyHelper.GetMbFromByte(gif.Size ?? 0), MediaType.Video, gif.Width, gif.Height) }));
    }


    /// <summary>
    ///     Function for get content url for redgif.
    /// </summary>
    /// <param name="url">Url redgif.</param>
    /// <param name="header">Header post.</param>
    /// <returns>Content post.</returns>
    private Task<(string Error, ProxyResponseContent? Content)> GetContentRedGif(string? url, string header)
    {
        if (url is null) return new Task<(string Error, ProxyResponseContent? Content)>(() => ("not valid url", null));
        var id = url[(url.LastIndexOf("/") + 1)..];
        return GetContentRedGfyId(id, header);
    }

    /// <summary>
    ///     Function for get content id for redgif.
    /// </summary>
    /// <param name="id">Id redgif.</param>
    /// <param name="header">Header reddit.</param>
    /// <returns>Content post.</returns>
    private async Task<(string Error, ProxyResponseContent? Content)> GetContentRedGfyId(string? id, string header)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri($"https://api.redgifs.com/v2/gifs/{id?.ToLower()}"),
        };
        client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var productValue = new ProductInfoHeaderValue("BotGetByLink", "1.0");

        client.DefaultRequestHeaders.UserAgent.Add(productValue);

        var request = new HttpRequestMessage(HttpMethod.Get, string.Empty);

        var response = await client.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();

        var redGiftData = JsonSerializer.Deserialize<RedGifData>(text);
        if (redGiftData is null || redGiftData.Gif is null || redGiftData.Gif.Verified) return ("not found", null);

        long size;
        if (redGiftData.Gif.HasAudio)
        {
            var videoUrl = redGiftData.Gif.Urls?["sd"];
            if (videoUrl is null || string.IsNullOrWhiteSpace(videoUrl)) return ("not found", null);
            size = await ProxyHelper.GetSizeContentUrlAsync(videoUrl);
            return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
                new[] { new MediaInfo(videoUrl, size, MediaType.Video, redGiftData.Gif.Width, redGiftData.Gif.Height) }));
        }

        var gifUrl = redGiftData.Gif.Urls?["hd"];
        if (gifUrl is null || string.IsNullOrWhiteSpace(gifUrl)) return ("not found", null);
        size = await ProxyHelper.GetSizeContentUrlAsync(gifUrl);
        return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
            new[] { new MediaInfo(gifUrl, size, MediaType.Video, redGiftData.Gif.Width, redGiftData.Gif.Height) }));
    }

    /// <summary>
    ///     Function for get contetn imgur.
    /// </summary>
    /// <param name="postData"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    private async Task<(string Error, ProxyResponseContent? Content)> GetContentImgur(RedditPostData postData, string header)
    {
        if (postData.Url is null) return ("not valid url", null);
        long size;
        if (picturesRegex.IsMatch(postData.Url, RegexOptions.IgnoreCase))
        {
            var source = postData.Preview?.Images?.FirstOrDefault()?.Source;
            if (source is null || string.IsNullOrWhiteSpace(source.Url))
            {
                size = await ProxyHelper.GetSizeContentUrlAsync(postData.Url);
                return ("Ok", new ProxyResponseContent(string.Empty, header,
                    new[] { new MediaInfo(postData.Url, size, MediaType.Photo) }));
            }

            size = await ProxyHelper.GetSizeContentUrlAsync(source.Url);
            return ("Ok", new ProxyResponseContent(string.Empty, header,
                new[] { new MediaInfo(source.Url, size, MediaType.Photo, source.Width, source.Height) }));
        }

        if (gifRegex.IsMatch(postData.Url?.ToLower()) || gifvRegex.IsMatch(postData.Url?.ToLower()))
        {
            var source = postData.Preview?.RedditVideoPreview; // trick
            var urlMp4 = $"{postData.Url[..(postData.Url.LastIndexOf("."))]}.mp4";
            size = await ProxyHelper.GetSizeContentUrlAsync(urlMp4);
            if (source is null)
            {
                return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
                    new[] { new MediaInfo(urlMp4, size, MediaType.Video) }));
            }

            return ("Ok", new ProxyResponseContent(string.Empty, header, UrlVideo:
                new[] { new MediaInfo(urlMp4, size, MediaType.Video, source.Width, source.Height) }));
        }

        var id = postData.Url[(postData.Url.LastIndexOf("/") + 1)..];
        var modify = postData.Url[(postData.Url.LastIndexOf("/") - 1)];
        if (modify == 'a')
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://api.imgur.com/3/album/{id}"),
            };

            client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {ImgurAppId}");
            var request = new HttpRequestMessage(HttpMethod.Get, string.Empty);

            var response = await client.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();

            var imgurAlbum = JsonSerializer.Deserialize<ImgurAlbum>(text);

            var media = imgurAlbum?.Data?.Images?.Select(x => new MediaInfo(x.Link, ProxyHelper.GetMbFromByte(x.Size ?? 0), x.Type?.Contains("image") ?? false ? MediaType.Photo : MediaType.Video, x.Width, x.Height));
            if (media is null || !media.Any()) 
                return ("Ok", new ProxyResponseContent($"{(string.IsNullOrWhiteSpace(postData.Url) ? string.Empty : $"{postData.Url}\n")}{postData.SelfText}", header));
            return ("Ok", new ProxyResponseContent(string.Empty, header, media.Where(x => x.Type == MediaType.Photo), media.Where(x => x.Type != MediaType.Photo)));
        }
        return ("Ok", new ProxyResponseContent($"{(string.IsNullOrWhiteSpace(postData.Url) ? string.Empty : $"{postData.Url}\n")}{postData.SelfText}", header));
    }
}