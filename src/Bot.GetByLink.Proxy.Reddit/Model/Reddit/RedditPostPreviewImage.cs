using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for preview image.
/// </summary>
public sealed class RedditPostPreviewImage
{
    /// <summary>
    ///     Gets source preview images.
    /// </summary>
    [JsonPropertyName("source")]
    public RedditPostPreviewImageSource? Source { get; init; } = null;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}