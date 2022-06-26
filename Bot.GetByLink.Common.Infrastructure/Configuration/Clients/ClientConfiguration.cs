using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Clients;

/// <summary>
///     Client Configuration.
/// </summary>
public sealed class ClientConfiguration
{
    /// <summary>
    ///     Gets access token.
    /// </summary>
    [JsonPropertyName("Token")]
    public string Token { get; init; } = string.Empty;

    /// <summary>
    ///     Gets chat for logging issues (database not used).
    /// </summary>
    [JsonPropertyName("ChatIdLog")]
    public string ChatIdLog { get; init; } = string.Empty;
}