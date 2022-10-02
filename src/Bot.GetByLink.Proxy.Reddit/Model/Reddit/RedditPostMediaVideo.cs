using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for media reddit video.
/// </summary>
public sealed class RedditPostMediaVideo
{
    /// <summary>
    ///     Gets fallback url video.
    /// </summary>
    [JsonPropertyName("fallback_url")]
    public string? FallbackUrl { get; init; } = null;

    /// <summary>
    ///     Gets height video.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; init; } = 0;

    /// <summary>
    ///     Gets width video.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; init; } = 0;

    /// <summary>
    ///     Gets thumbnail url.
    /// </summary>
    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; init; } = null;

    /// <summary>
    ///     Gets thumbnail height video.
    /// </summary>
    [JsonPropertyName("thumbnail_height")]
    public int ThumbnailHeight { get; init; } = 0;

    /// <summary>
    ///     Gets thumbnail width video.
    /// </summary>
    [JsonPropertyName("thumbnail_width")]
    public int ThumbnailWidth { get; init; } = 0;

    /// <summary>
    ///     Gets flag gif.
    /// </summary>
    [JsonPropertyName("is_gif")]
    public bool? IsGif { get; init; } = false;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}