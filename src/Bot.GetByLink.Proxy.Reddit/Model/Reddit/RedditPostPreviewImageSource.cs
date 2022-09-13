using System.Text.Json.Serialization;
using System.Web;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for source image preview.
/// </summary>
public sealed class RedditPostPreviewImageSource
{
    private readonly string? url = string.Empty;

    /// <summary>
    ///     Gets url image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url
    {
        get => url;
        init => url = HttpUtility.HtmlDecode(value);
    }

    /// <summary>
    ///     Gets height image.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; init; } = 0;

    /// <summary>
    ///     Gets width image.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; init; } = 0;
}