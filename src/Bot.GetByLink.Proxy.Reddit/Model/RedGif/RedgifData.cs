using Bot.GetByLink.Proxy.Reddit.Model.GfyCat;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bot.GetByLink.Proxy.Reddit.Model.RedGif
{
    /// <summary>
    ///     Class for redgif data.
    /// </summary>
    public sealed class RedGifData
    {
        /// <summary>
        ///     Gets redgif gif.
        /// </summary>
        [JsonPropertyName("gif")]
        public RedGifItem? Gif { get; init; } = null;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
