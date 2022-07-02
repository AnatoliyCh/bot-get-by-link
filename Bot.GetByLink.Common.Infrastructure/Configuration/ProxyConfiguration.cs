using Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;
using Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration;

/// <summary>
///     Collection of proxy configurations.
/// </summary>
public sealed class ProxyConfiguration : IProxyConfiguration
{
    /// <summary>
    ///     Gets Reddit proxy configuration.
    /// </summary>
    [JsonPropertyName("Reddit")]
    public RedditConfiguration Reddit { get; init; } = new();

    /// <summary>
    ///     Gets Vk proxy configuration.
    /// </summary>
    [JsonPropertyName("Vk")]
    public VkConfiguration Vk { get; init; } = new();
}