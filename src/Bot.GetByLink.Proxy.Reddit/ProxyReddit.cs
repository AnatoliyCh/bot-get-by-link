using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Abstractions.Proxy;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Proxy;
using Bot.GetByLink.Common.Infrastructure.Regexs;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Common;
using Bot.GetByLink.Proxy.Reddit.Model;
using Bot.GetByLink.Proxy.Reddit.Regexs;
using Reddit;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
///     Reddit API for getting post content by id or url.
/// </summary>
public sealed class ProxyReddit : ProxyService
{
    private readonly string appId;
    private readonly IRegexWrapper galleryRegex;
    private readonly IRegexWrapper gifRegex;
    private readonly IRegexWrapper picturesRegex;
    private readonly string secretId;
    private readonly string urlBase = "www.reddit.com";
    private readonly string userAgent = "bot-get-by-link-web";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyReddit" /> class.
    /// </summary>
    /// <param name="configuration">Bot configuration.</param>
    public ProxyReddit(IBotConfiguration? configuration)
        : base(new[] { new RedditPostRegexWrapper() })
    {
        ArgumentNullException.ThrowIfNull(configuration);

        appId = configuration.Proxy.Reddit.AppId ?? string.Empty;
        secretId = configuration.Proxy.Reddit.Secret ?? string.Empty;
        picturesRegex = new PictureRegexWrapper();
        gifRegex = new GifRegexWrapper();
        galleryRegex = new RedditGalleryRegexWrapper();
    }

    /// <summary>
    ///     Method for getting reddit post content by post url.
    /// </summary>
    /// <param name="url">Url to a reddit post in the format https://www.reddit.com/r/S+/comments/S+.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override async Task<IProxyContent?> GetContentUrlAsync(string url)
    {
        var postId = galleryRegex.IsMatch(url) ? GetRedditId(url, "gallery/") : GetRedditId(url, "comments/");
        return await GetContentIdAsync(postId);
    }

    /// <summary>
    ///     Function for get reddit id.
    /// </summary>
    /// <param name="url">Url reddit post or gallery.</param>
    /// <param name="findString">gallery/ or comments/.</param>
    /// <returns>Reddit id.</returns>
    private string GetRedditId(string url, string findString)
    {
        var cutUrlPost = url[(url.IndexOf(findString) + findString.Length)..];
        var indexCutSlash = cutUrlPost.IndexOf("/");
        if (indexCutSlash == -1) return cutUrlPost;
        return cutUrlPost[..indexCutSlash];
    }

    /// <summary>
    ///     Method for getting reddit post content by post id.
    /// </summary>
    /// <param name="postId">Post ID.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    /// <exception cref="ArgumentNullException">Returned if an empty postId was passed.</exception>
    public async Task<IProxyContent> GetContentIdAsync(string postId)
    {
        if (string.IsNullOrWhiteSpace(postId)) throw new ArgumentNullException(nameof(postId), "Пустой id поста");

        var accsessToken = await GetAccessTokenAsync();
        var redditClient =
            new RedditClient(appId, appSecret: secretId, accessToken: accsessToken, userAgent: userAgent);
        var postNet = redditClient.LinkPost($"t3_{postId}").Info();
        var fullPostInfo = await GetFullPostInfo($"t3_{postId}");
        var postData = GetPostData(fullPostInfo);
        var crossPostId = GetParentPostIdAsync(postData);

        if (!string.IsNullOrWhiteSpace(crossPostId)) postNet = redditClient.LinkPost($"{crossPostId}").Info();
        var header = postNet.Title;

        if (postData?.FirstOrDefault(x => x.Key == "gallery_data").Value is not null)
        {
            var galleryMedia = await GetGalleryMedia(postData);
            return new ProxyResponseContent(string.Empty, header,
                galleryMedia);
        }

        if (postNet.Listing.Media == null && !postNet.Listing.IsVideo && !postNet.Listing.IsRedditMediaDomain &&
            !gifRegex.IsMatch(postNet.Listing.URL?.ToLower()))
            return new ProxyResponseContent(postNet.Listing.SelfText, header);

        long size;
        if (picturesRegex.IsMatch(postNet.Listing.URL, RegexOptions.IgnoreCase))
        {
            size = await ProxyHelper.GetSizeContentUrlAsync(postNet.Listing.URL);
            return new ProxyResponseContent(string.Empty, header,
                new[] { new MediaInfo(postNet.Listing.URL, size, MediaType.Photo) });
        }

        if (postNet.Listing.Media != null && postNet.Listing.IsVideo)
        {
            var videoLink = GetVideoLink(postNet.Listing.Media);
            if (!string.IsNullOrWhiteSpace(videoLink))
            {
                size = await ProxyHelper.GetSizeContentUrlAsync(videoLink);
                return new ProxyResponseContent(string.Empty, header, null,
                    new[] { new MediaInfo(videoLink, size, MediaType.Video) });
            }
        }

        if (gifRegex.IsMatch(postNet.Listing.URL.ToLower()))
        {
            size = await ProxyHelper.GetSizeContentUrlAsync(postNet.Listing.URL);
            return new ProxyResponseContent(string.Empty, header, null,
                new[] { new MediaInfo(postNet.Listing.URL, size, MediaType.Video) });
        }

        return new ProxyResponseContent(postNet.Listing.URL, header);
    }

    /// <summary>
    ///     Function for get url reddit video.
    /// </summary>
    /// <param name="media">Object media.</param>
    /// <returns>Url reddit video.</returns>
    private static string? GetVideoLink(object media)
    {
        if (media is not JsonObject mediaJsonObject) return string.Empty;
        return mediaJsonObject?.FirstOrDefault(x => x.Key == "reddit_video").Value?.GetValue<JsonObject>()
            .FirstOrDefault(x => x.Key == "fallback_url").Value?.GetValue<string>();
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
            JsonSerializer.Deserialize<AccessTokenReddit>(await response.Content.ReadAsStringAsync());
        if (accsessTokenObject == null || string.IsNullOrWhiteSpace(accsessTokenObject.AccessToken))
            throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
        return accsessTokenObject.AccessToken;
    }

    /// <summary>
    ///     Function for get json post.
    /// </summary>
    /// <param name="postId">Id post.</param>
    /// <returns>Post in json.</returns>
    private async Task<JsonObject?> GetFullPostInfo(string postId)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri($"https://{urlBase}")
        };
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/info.json?id={postId}");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var text = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonObject>(text);
    }

    /// <summary>
    /// </summary>
    /// <param name="post"></param>
    /// <returns></returns>
    private JsonObject? GetPostData(JsonObject post)
    {
        return post?.FirstOrDefault(x => x.Key == "data").Value?
            .AsObject()?.FirstOrDefault(x => x.Key == "children").Value?
            .AsArray()?.FirstOrDefault()?
            .AsObject().FirstOrDefault(x => x.Key == "data").Value?
            .AsObject();
    }

    /// <summary>
    ///     Function for get parent post.
    /// </summary>
    /// <param name="post">Post in json.</param>
    /// <returns>Id parent post if there is parent post.</returns>
    private string? GetParentPostIdAsync(JsonObject post)
    {
        return post.FirstOrDefault(x => x.Key == "crosspost_parent").Value?
            .GetValue<string>();
    }

    /// <summary>
    ///     Function for get gallery media.
    /// </summary>
    /// <param name="post">Post in json.</param>
    /// <returns>List media info </returns>
    private async Task<IEnumerable<IMediaInfo>> GetGalleryMedia(JsonObject post)
    {
        var galleryData = post?.FirstOrDefault(x => x.Key == "gallery_data").Value?.Deserialize<RedditGalleryData>();
        var mediaData = post?.FirstOrDefault(x => x.Key == "media_metadata").Value
            ?.Deserialize<Dictionary<string, RedditMediaMetaDataItem>>();
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
                media.Value.SourceElements.Width ?? 0, media.Value.SourceElements.Height ?? 0));
    }
}