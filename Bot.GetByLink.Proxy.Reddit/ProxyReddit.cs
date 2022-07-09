﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Infrastructure;
using Bot.GetByLink.Common.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reddit;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
///     Reddit API for getting post content by id or url.
/// </summary>
public class ProxyReddit : ProxyService
{
    private readonly string appId;
    private readonly string secretId;
    private readonly string uriString = "www.reddit.com";
    private readonly string userAgent = "bot-get-by-link-web";

    /// <summary>
    ///    Function for get url reddit video.
    /// </summary>
    /// <param name="media">Object media.</param>
    /// <returns>Url reddit video.</returns>
    private static string? GetVideoLink(object media)
    {
        if (media is not JObject mediaJsonObject) return string.Empty;
        return mediaJsonObject?.SelectToken("reddit_video")?.SelectToken("fallback_url")?.Value<string>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyReddit" /> class.
    /// </summary>
    /// <param name="appId">Id application Reddit.</param>
    /// <param name="secretId">Secret Id application Reddit.</param>
    /// <param name="regexUrl">Regex url for proxy.</param>
    public ProxyReddit(string[] regexUrl)
        : base(regexUrl)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables().Build();

        appId = configuration.GetValue<string>("reddit:app-id") ?? string.Empty;
        secretId = configuration.GetValue<string>("reddit:secret") ?? string.Empty;
    }

    /// <summary>
    ///     Method for getting reddit post content by post url.
    /// </summary>
    /// <param name="url">Url to a reddit post in the format https://www.reddit.com/r/S+/comments/S+.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override async Task<ProxyResponseContent> GetContentUrlAsync(string url)
    {
        var cutUrlPost = url[(url.IndexOf("comments/") + "comments/".Length)..];
        var postId = cutUrlPost[..cutUrlPost.IndexOf("/")];
        return await GetContentIdAsync(postId);
    }

    /// <summary>
    ///     Method for getting reddit post content by post id.
    /// </summary>
    /// <param name="postId">Post ID.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    /// <exception cref="ArgumentNullException">Returned if an empty postId was passed.</exception>
    public async Task<ProxyResponseContent> GetContentIdAsync(string postId)
    {
        if (string.IsNullOrWhiteSpace(postId)) throw new ArgumentNullException(nameof(postId), "Пустой id поста");

        var accsessToken = await GetAccessTokenAsync();
        var redditClient =
            new RedditClient(appId, appSecret: secretId, accessToken: accsessToken, userAgent: userAgent);
        var post = redditClient.LinkPost($"t3_{postId}").Info();
        var crossPostId = await GetParentPostIdAsync($"t3_{postId}");
        if (!string.IsNullOrWhiteSpace(crossPostId)) post = redditClient.LinkPost($"{crossPostId}").Info();
        var startText = $"https://www.reddit.com{post.Permalink}\n\n{post.Title}\n";
        if (post.Listing.Media == null && !post.Listing.IsVideo && !post.Listing.IsRedditMediaDomain)
        {
            return new ProxyResponseContent($"{startText}{post.Listing.SelfText}", Array.Empty<string>(), Array.Empty<string>());
        }

        if (Regex.IsMatch(post.Listing.URL, @"https?://\S+(?:jpg|jpeg|png)", RegexOptions.IgnoreCase))
            return new ProxyResponseContent(startText, new string[] { post.Listing.URL }, Array.Empty<string>());
        if (post.Listing.Media != null && post.Listing.IsVideo)
        {
            var videoLink = GetVideoLink(post.Listing.Media);
            if (!string.IsNullOrWhiteSpace(videoLink)) return new ProxyResponseContent(startText, Array.Empty<string>(), new string[] { videoLink });
            return new ProxyResponseContent(startText, Array.Empty<string>(), Array.Empty<string>());
        }

        return new ProxyResponseContent(startText, Array.Empty<string>(), new string[] { post.Listing.URL });
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
        var client = new HttpClient();
        client.BaseAddress = new Uri($"https://{uriString}");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/access_token");

        var byteArray = new UTF8Encoding().GetBytes($"{appId}:{secretId}");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var formData = new List<KeyValuePair<string, string>>();
        formData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        formData.Add(new KeyValuePair<string, string>("device_id", appId));

        request.Content = new FormUrlEncodedContent(formData);
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var accsessTokenObject =
            JsonConvert.DeserializeObject<AccessTokenReddit>(await response.Content.ReadAsStringAsync());
        if (accsessTokenObject == null || string.IsNullOrWhiteSpace(accsessTokenObject.AccessToken))
            throw new HttpRequestException($"Реддит вернул ошибку: {response.Content.ReadAsStringAsync().Result}");
        return accsessTokenObject.AccessToken;
    }

    /// <summary>
    ///     Function for get parent post.
    /// </summary>
    /// <param name="postId">Id cross post.</param>
    /// <returns>Id parent post if there is parent post.</returns>
    private async Task<string?> GetParentPostIdAsync(string postId)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri($"https://{uriString}");
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/info.json?id={postId}");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var text = await response.Content.ReadAsStringAsync();
        var data = (JObject)JsonConvert.DeserializeObject(text);
        string? prarentPostId = data?.SelectToken("data")?.SelectToken("children")?.First["data"]?.SelectToken("crosspost_parent")?.Value<string>();
        return prarentPostId;
    }
}