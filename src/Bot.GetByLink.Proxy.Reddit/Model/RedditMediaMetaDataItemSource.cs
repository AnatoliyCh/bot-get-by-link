using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace Bot.GetByLink.Proxy.Reddit.Model
{
    /// <summary>
    ///     Class for media metadata item source.
    /// </summary>
    public sealed class RedditMediaMetaDataItemSource
    {
        [JsonIgnore]
        private string? url = string.Empty;
        private string? gif = string.Empty;
        private string? mp4 = string.Empty;

        /// <summary>
        ///     Gets height element.
        /// </summary>
        [JsonPropertyName("y")]
        public int? Height { get; init; } = 0;

        /// <summary>
        ///     Gets width element.
        /// </summary>
        [JsonPropertyName("x")]
        public int? Width { get; init; } = 0;

        /// <summary>
        ///     Gets width element.
        /// </summary>
        [JsonPropertyName("gif")]
        public string? Gif
        {
            get => gif; init => gif = HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        ///     Gets width element.
        /// </summary>
        [JsonPropertyName("mp4")]
        public string? Mp4
        {
            get => mp4; init => mp4 = HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        ///     Gets width element.
        /// </summary>
        [JsonPropertyName("u")]
        public string? Url
        {
            get => url; init => url = HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        ///     Gets or sets size element.
        /// </summary>
        [JsonIgnore]
        public double Size { get; set; } = 0;
    }
}
