using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bot.GetByLink.Proxy.Reddit.Model.GfyCat
{
    /// <summary>
    ///     Class for gfycat data item.
    /// </summary>
    public sealed class GfyItem
    {
        /// <summary>
        ///     Gets item content.
        /// </summary>
        [JsonPropertyName("content_urls")]
        public IDictionary<string, GfyCatItemContent>? ContentUrls { get; init; } = null;

        /// <summary>
        ///     Gets a value indicating whether item has audio.
        /// </summary>
        [JsonPropertyName("hasAudio")]
        public bool HasAudio { get; init; } = false;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
