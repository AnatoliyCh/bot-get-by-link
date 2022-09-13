using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit
{
    /// <summary>
    ///     Class for preview reddit post.
    /// </summary>
    public sealed class RedditPostPreview
    {
        /// <summary>
        ///     Gets preview images.
        /// </summary>
        [JsonPropertyName("images")]
        public IList<RedditPostPreviewImage>? Images { get; init; } = null;

        /// <summary>
        ///     Gets preview video.
        /// </summary>
        [JsonPropertyName("reddit_video_preview")]
        public RedditPostMediaVideo? RedditVideoPreview { get; init; } = null;

        /// <summary>
        ///     Gets unknow fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
    }
}
