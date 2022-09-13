using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bot.GetByLink.Proxy.Reddit.Model.Imgur
{
    /// <summary>
    ///     Class for imgur album.
    /// </summary>
    public sealed class ImgurAlbum
    {
        /// <summary>
        ///     Gets imgur album data.
        /// </summary>
        [JsonPropertyName("data")]
        public ImgurAlbumData? Data { get; init; } = null;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
