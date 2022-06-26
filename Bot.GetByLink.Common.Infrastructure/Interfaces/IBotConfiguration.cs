using Bot.GetByLink.Common.Infrastructure.Configuration;

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
    public ClientsConfiguration Clients { get; init; }

    /// <summary>
    ///     Gets сollection of proxy configurations.
    /// </summary>
    public ProxyConfiguration Proxy { get; init; }
}