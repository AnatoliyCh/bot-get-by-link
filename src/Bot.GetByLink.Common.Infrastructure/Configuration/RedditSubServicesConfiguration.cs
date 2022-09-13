using Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration
{
    /// <summary>
    ///     Reddit proxy configuration.
    /// </summary>
    public class RedditSubServicesConfiguration : IRedditSubServicesConfiguration
    {
        /// <summary>
        ///     Gets secret key.
        /// </summary>
        [JsonPropertyName("Imgur")]
        public IImgurConfiguration? Imgur { get; init; } = new ImgurConfiguration();
    }
}
