using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model;
using Newtonsoft.Json;
using Reddit;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
///     Reddit API for getting post content by id or url.
/// </summary>
public sealed class ProxyReddit : ProxyService
{
    private readonly string appId;
    private readonly string secretId;
    private readonly string urlBase = "www.reddit.com";
    private readonly string userAgent = "bot-get-by-link-web";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyReddit" /> class.
    /// </summary>
    /// <param name="configuration">Bot configuration.</param>
    public ProxyReddit(IBotConfiguration? configuration)
        : base(new[]
        {
            @"https?:\/\/www.reddit.com\/r\/\S+/comments\/\S+"
        }) // TODO: переделать на фабрику / найти подходящее место.
    {
        ArgumentNullException.ThrowIfNull(configuration);

        appId = configuration.Proxy.Reddit.AppId ?? string.Empty;
        secretId = configuration.Proxy.Reddit.Secret ?? string.Empty;
    }

    /// <summary>
    ///     Method for getting reddit post content by post url.
    /// </summary>
    /// <param name="url">Url to a reddit post in the format https://www.reddit.com/r/S+/comments/S+.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override async Task<ProxyResponseContent> GetContentUrl(string url)
    {
        var cutUrlPost = url[(url.IndexOf("comments/") + "comments/".Length)..];
        var postId = cutUrlPost[..cutUrlPost.IndexOf("/")];
        return await GetContentId(postId);
    }

    /// <summary>
    ///     Method for getting reddit post content by post id.
    /// </summary>
    /// <param name="postId">Post ID.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    /// <exception cref="ArgumentNullException">Returned if an empty postId was passed.</exception>
    public async Task<ProxyResponseContent> GetContentId(string postId)
    {
        if (string.IsNullOrWhiteSpace(postId)) throw new ArgumentNullException(nameof(postId), "Пустой id поста");

        var accsessToken = await GetAccessToken();
        var redditAccess =
            new RedditClient(appId, appSecret: secretId, accessToken: accsessToken, userAgent: userAgent);
        var post = redditAccess.LinkPost($"t3_{postId}").Info();
        if (post.Listing.Media == null && !post.Listing.IsVideo && !post.Listing.IsRedditMediaDomain)
            return new ProxyResponseContent { Text = $"{post.Listing.URL}\n\n{post.Listing.SelfText}" };
        if (Regex.IsMatch(post.Listing.URL, @"https?://\S+(?:jpg|jpeg|png)", RegexOptions.IgnoreCase))
            return new ProxyResponseContent { Text = post.Listing.SelfText, UrlPicture = post.Listing.URL };
        return new ProxyResponseContent { Text = post.Listing.SelfText, UrlVideo = post.Listing.URL };
    }

    /// <summary>
    ///     Function for get accsess token from reddit.
    /// </summary>
    /// <returns>Accsess token reddit.</returns>
    /// <exception cref="HttpRequestException">
    ///     Returned if it was not possible to authorize in reddit or it returned empty
    ///     answer.
    /// </exception>
    private async Task<string> GetAccessToken()
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
            JsonConvert.DeserializeObject<AccessTokenReddit>(await response.Content.ReadAsStringAsync());
        if (accsessTokenObject == null || string.IsNullOrWhiteSpace(accsessTokenObject.AccessToken))
            throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
        return accsessTokenObject.AccessToken;
    }
}