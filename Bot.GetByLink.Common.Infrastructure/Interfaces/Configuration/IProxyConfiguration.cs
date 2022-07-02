using Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;

/// <summary>
///     Collection of proxy configurations interface.
/// </summary>
public interface IProxyConfiguration
{
    /// <summary>
    ///     Gets Reddit proxy configuration.
    /// </summary>
    public RedditConfiguration Reddit { get; init; }

    /// <summary>
    ///     Gets Vk proxy configuration.
    /// </summary>
    public VkConfiguration Vk { get; init; }
}