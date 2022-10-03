using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Imgur;

/// <summary>
///     Class for imgur album image.
/// </summary>
public sealed class ImgurAlbumImage
{
    /// <summary>
    ///     Gets width content.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; init; } = 0;

    /// <summary>
    ///     Gets height content.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; init; } = 0;

    /// <summary>
    ///     Gets type content.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; } = string.Empty;

    /// <summary>
    ///     Gets size content.
    /// </summary>
    [JsonPropertyName("size")]
    public int? Size { get; init; } = -1;

    /// <summary>
    ///     Gets a value indicating whether item has audio.
    /// </summary>
    [JsonPropertyName("hasAudio")]
    public bool? HasAudio { get; init; } = false;

    /// <summary>
    ///     Gets a value indicating whether content is animated.
    /// </summary>
    [JsonPropertyName("animated")]
    public bool? IsAnimated { get; init; } = false;

    /// <summary>
    ///     Gets link content.
    /// </summary>
    [JsonPropertyName("link")]
    public string? Link { get; init; } = string.Empty;
}