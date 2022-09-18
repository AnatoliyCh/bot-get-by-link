using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.GetByLink.Proxy.Reddit.Model.Streamable
{
    /// <summary>
    ///     Class for streamable file.
    /// </summary>
    public sealed class StreamableFile
    {
        /// <summary>
        ///     Gets url.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; init; } = null;

        /// <summary>
        ///     Gets height.
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; init; } = null;

        /// <summary>
        ///     Gets width.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; init; } = null;

        /// <summary>
        ///     Gets size.
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get; init; } = null;
    }
}
