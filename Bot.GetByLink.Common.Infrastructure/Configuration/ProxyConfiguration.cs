using Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;
using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration;

/// <summary>
///     Collection of proxy configurations.
/// </summary>
public sealed class ProxyConfiguration : Interfaces.Configuration.IProxyConfiguration
{
    /// <summary>
    ///     Gets Reddit proxy configuration.
    /// </summary>
    [JsonPropertyName("Reddit")]
    public IRedditConfiguration Reddit { get; init; } = new RedditConfiguration();

    /// <summary>
    ///     Gets Vk proxy configuration.
    /// </summary>
    [JsonPropertyName("Vk")]
    public IVkConfiguration Vk { get; init; } = new VkConfiguration();
}
