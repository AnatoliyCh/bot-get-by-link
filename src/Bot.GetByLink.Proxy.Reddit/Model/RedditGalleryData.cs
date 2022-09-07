using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.GetByLink.Proxy.Reddit.Model
{
    /// <summary>
    ///     Class gallery data.
    /// </summary>
    public sealed class RedditGalleryData
    {
        /// <summary>
        ///     Gets item gallery.
        /// </summary>
        [JsonPropertyName("items")]
        public IList<RedditGalleryItem>? Items { get; init; } = null;
    }
}
