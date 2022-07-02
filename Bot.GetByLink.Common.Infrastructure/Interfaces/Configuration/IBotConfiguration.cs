using Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Bot сonfiguration interface.
/// </summary>
public interface IBotConfiguration
{
    /// <summary>
    ///     Gets name of the project being launched.
    /// </summary>
    public string ProjectName { get; init; }

    /// <summary>
    ///     Gets сollection of client configurations.
    /// </summary>
    public IClientConfiguration Clients { get; init; }

    /// <summary>
    ///     Gets сollection of proxy configurations.
    /// </summary>
    public IProxyConfiguration Proxy { get; init; }
}