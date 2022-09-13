using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.GfyCat
{
    /// <summary>
    ///     Class for gfycat content item.
    /// </summary>
    public sealed class GfyCatItemContent
    {
        /// <summary>
        ///     Gets url content.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; init; } = null;

        /// <summary>
        ///     Gets size content.
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get; init; } = -1;

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
    }
}
