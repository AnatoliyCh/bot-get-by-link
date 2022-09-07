using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model;

/// <summary>
///     Class gallery data.
/// </summary>
public sealed class RedditGalleryData
{
    /// <summary>
    ///     Gets item gallery.
    /// </summary>
    [JsonPropertyName("items")]
    public IList<RedditGalleryItem>? Items { get; init; } = null;
}