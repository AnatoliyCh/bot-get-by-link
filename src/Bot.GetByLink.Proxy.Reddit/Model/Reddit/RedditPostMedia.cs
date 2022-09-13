using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for reddit post media / secured media.
/// </summary>
public sealed class RedditPostMedia
{
    /// <summary>
    ///     Gets reddit video.
    /// </summary>
    [JsonPropertyName("reddit_video")]
    public RedditPostMediaVideo? RedditVideo { get; init; } = null;

    /// <summary>
    ///     Gets reddit video.
    /// </summary>
    [JsonPropertyName("oembed")]
    public RedditPostMediaVideo? Oembed { get; init; } = null;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}