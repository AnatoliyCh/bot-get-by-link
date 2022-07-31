using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;

namespace Bot.GetByLink.Common.Infrastructure.Model.Configuration;

/// <summary>
///     Bot сonfiguration class.
/// </summary>
public sealed class BotConfiguration : IBotConfiguration
{
    /// <summary>
    ///     Gets name of the project being launched.
    /// </summary>
    [JsonPropertyName("ProjectName")]
    public string ProjectName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets сollection of client configurations.
    /// </summary>
    [JsonPropertyName("Clients")]
    public IClientConfiguration Clients { get; init; } = new ClientsConfiguration();

    /// <summary>
    ///     Gets сollection of proxy configurations.
    /// </summary>
    [JsonPropertyName("Proxy")]
    public IProxyConfiguration Proxy { get; init; } = new ProxyConfiguration();
}