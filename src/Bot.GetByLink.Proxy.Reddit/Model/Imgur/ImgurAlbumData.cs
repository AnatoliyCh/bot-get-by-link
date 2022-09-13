using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Imgur;

/// <summary>
///     Class for imgur album data.
/// </summary>
public sealed class ImgurAlbumData
{
    /// <summary>
    ///     Gets imgur album images.
    /// </summary>
    [JsonPropertyName("images")]
    public IList<ImgurAlbumImage>? Images { get; init; } = null;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}