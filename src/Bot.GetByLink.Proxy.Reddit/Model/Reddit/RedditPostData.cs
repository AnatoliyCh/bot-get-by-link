using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for reddit post data.
/// </summary>
public sealed class RedditPostData
{
    /// <summary>
    ///     Gets children reddit post.
    /// </summary>
    [JsonPropertyName("children")]
    public IList<RedditPost>? Children { get; init; } = null;

    /// <summary>
    ///     Gets a value indicating whether flag is gallery.
    /// </summary>
    [JsonPropertyName("is_gallery")]
    public bool IsGallery { get; init; } = false;

    /// <summary>
    ///     Gets title post.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; } = null;

    /// <summary>
    ///     Gets media metadata.
    /// </summary>
    [JsonPropertyName("media_metadata")]
    public Dictionary<string, RedditMediaMetaDataItem>? MediaMetadata { get; init; } = null;

    /// <summary>
    ///     Gets domain.
    /// </summary>
    [JsonPropertyName("domain")]
    public string? Domain { get; init; } = null;

    /// <summary>
    ///     Gets gallery data.
    /// </summary>
    [JsonPropertyName("gallery_data")]
    public RedditGalleryData? GalleryData { get; init; } = null;

    /// <summary>
    ///     Gets id post.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; } = null;

    /// <summary>
    ///     Gets url source.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; } = null;

    /// <summary>
    ///     Gets a value indicating whether flag is video.
    /// </summary>
    [JsonPropertyName("is_video")]
    public bool IsVideo { get; init; } = false;

    /// <summary>
    ///     Gets id crosspost parent post.
    /// </summary>
    [JsonPropertyName("crosspost_parent")]
    public string? CrosspostParent { get; init; } = null;

    /// <summary>
    ///     Gets media.
    /// </summary>
    [JsonPropertyName("media")]
    public RedditPostMedia? Media { get; init; } = null;

    /// <summary>
    ///     Gets secure media.
    /// </summary>
    [JsonPropertyName("secure_media")]
    public RedditPostMedia? SecureMedia { get; init; } = null;

    /// <summary>
    ///     Gets a value indicating whether flag is reddit media domain.
    /// </summary>
    [JsonPropertyName("is_reddit_media_domain")]
    public bool IsRedditMediaDomain { get; init; } = false;

    /// <summary>
    ///     Gets self text post.
    /// </summary>
    [JsonPropertyName("selftext")]
    public string SelfText { get; init; } = string.Empty;

    /// <summary>
    ///     Gets preview.
    /// </summary>
    [JsonPropertyName("preview")]
    public RedditPostPreview? Preview { get; init; } = null;

    /// <summary>
    ///     Gets preview.
    /// </summary>
    [JsonPropertyName("crosspost_parent_list")]
    public IList<RedditPostData>? СrosspostParentList { get; init; } = null;

    /// <summary>
    ///     Gets a value indicating whether flag nsfw.
    /// </summary>
    [JsonPropertyName("over_18")]
    public bool Over18 { get; init; } = false;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}