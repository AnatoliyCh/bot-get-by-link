using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Configuration;

/// <summary>
///     Collection of proxy configurations.
/// </summary>
public sealed class ProxyConfiguration
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