using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model;

/// <summary>
///     Class item gallery.
/// </summary>
public sealed class RedditGalleryItem
{
    /// <summary>
    ///     Gets caption item gallery.
    /// </summary>
    [JsonPropertyName("caption")]
    public string? Caption { get; init; } = string.Empty;

    /// <summary>
    ///     Gets mediaIid item gallery.
    /// </summary>
    [JsonPropertyName("media_id")]
    public string? MediaId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets id item gallery.
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; } = 0;
}