using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bot.GetByLink.Proxy.Reddit.Model.RedGif
{
    /// <summary>
    ///     Class for redgif item(gif).
    /// </summary>
    public sealed class RedGifItem
    {
        /// <summary>
        ///     Gets item content.
        /// </summary>
        [JsonPropertyName("urls")]
        public IDictionary<string, string>? Urls { get; init; } = null;

        /// <summary>
        ///     Gets a value indicating whether item has audio.
        /// </summary>
        [JsonPropertyName("hasAudio")]
        public bool HasAudio { get; init; } = false;

        /// <summary>
        ///     Gets width content.
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get; init; } = 0;

        /// <summary>
        ///     Gets height content.
        /// </summary>
        [JsonPropertyName("height")]
        public int Height { get; init; } = 0;

        /// <summary>
        ///     Gets a value indicating whether verifed post.
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; init; } = false;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
