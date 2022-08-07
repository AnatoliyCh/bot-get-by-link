using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

namespace Bot.GetByLink.Common.Interfaces.Configuration;

/// <summary>
///     Collection of proxy configurations interface.
/// </summary>
public interface IProxyConfiguration
{
    /// <summary>
    ///     Gets Reddit proxy configuration.
    /// </summary>
    public IRedditConfiguration Reddit { get; init; }

    /// <summary>
    ///     Gets Vk proxy configuration.
    /// </summary>
    public IVkConfiguration Vk { get; init; }
}
