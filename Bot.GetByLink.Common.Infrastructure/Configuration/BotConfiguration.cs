using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Configuration;

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
    public ClientsConfiguration Clients { get; init; } = new();

    /// <summary>
    ///     Gets сollection of proxy configurations.
    /// </summary>
    [JsonPropertyName("Proxy")]
    public ProxyConfiguration Proxy { get; init; } = new();
}