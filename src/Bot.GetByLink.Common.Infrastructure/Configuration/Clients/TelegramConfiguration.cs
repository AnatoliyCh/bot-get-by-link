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
    ///     Gets a value indicating whether specifies whether to discard all pending updates before polling starts.
    /// </summary>
    [JsonPropertyName("ThrowPendingUpdates")]
    public bool ThrowPendingUpdates { get; init; } = false;

    /// <summary>
    ///     Gets mediaGroup send delay in milliseconds.
    /// </summary>
    [JsonPropertyName("DelaySendingMediaGroupMilliseconds")]
    public int DelaySendingMediaGroupMilliseconds { get; init; } = 30000;

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
    ///     Gets max MediaGroup lenght for message.
    /// </summary>
    [JsonPropertyName("MaxColMediaInMessage")]
    public int MaxColMediaInMessage { get; init; } = 1;

    /// <summary>
    ///     Gets max media size px for message.
    /// </summary>
    [JsonPropertyName("MaxSizePxMedia")]
    public int MaxSizePxMedia { get; init; } = 1;
}