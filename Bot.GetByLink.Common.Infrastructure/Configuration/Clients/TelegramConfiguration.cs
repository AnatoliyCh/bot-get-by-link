using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Interfaces.Configuration.Clients;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Clients;

/// <summary>
///     Client Configuration.
/// </summary>
public sealed class TelegramConfiguration : ITelegramConfiguration
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

    /// <summary>
    ///     Gets max size in mb for one photo in message.
    /// </summary>
    [JsonPropertyName("MaxSizeMbPhoto")]
    public double MaxSizeMbPhoto { get; init; } = 0;

    /// <summary>
    ///     Gets max size in mb for one video in message.
    /// </summary>
    [JsonPropertyName("MaxSizeMbVideo")]
    public double MaxSizeMbVideo { get; init; } = 0;

    /// <summary>
    ///     Gets max text lenght for caption first  media.
    /// </summary>
    [JsonPropertyName("MaxTextLenghtFirstMedia")]
    public int MaxTextLenghtFirstMedia { get; init; } = 0;

    /// <summary>
    ///     Gets max text lenght for message.
    /// </summary>
    [JsonPropertyName("MaxTextLenghtMessage")]
    public int MaxTextLenghtMessage { get; init; } = 0;

    /// <summary>
    ///     Gets max text lenght for message.
    /// </summary>
    [JsonPropertyName("MaxColMediaImMessage")]
    public int MaxColMediaImMessage { get; init; } = 0;
}