using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;
using Bot.GetByLink.Common.Infrastructure.Model.Configuration.Clients;

namespace Bot.GetByLink.Common.Infrastructure.Model.Configuration;

/// <summary>
///     Collection of client configurations.
/// </summary>
public sealed class ClientsConfiguration : IClientConfiguration
{
    /// <summary>
    ///     Gets Telegram client configuration.
    /// </summary>
    [JsonPropertyName("Telegram")]
    public ClientConfiguration Telegram { get; init; } = new();

    /// <summary>
    ///     Gets Discord client configuration.
    /// </summary>
    [JsonPropertyName("Discord")]
    public ClientConfiguration Discord { get; init; } = new();
}