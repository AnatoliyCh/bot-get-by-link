using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Infrastructure.Configuration.Clients;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Configuration.Clients;

namespace Bot.GetByLink.Common.Infrastructure.Configuration;

/// <summary>
///     Collection of client configurations.
/// </summary>
public sealed class ClientsConfiguration : IClientConfiguration
{
    /// <summary>
    ///     Gets Telegram client configuration.
    /// </summary>
    [JsonPropertyName("Telegram")]
    public ITelegramConfiguration Telegram { get; init; } = new TelegramConfiguration();
}