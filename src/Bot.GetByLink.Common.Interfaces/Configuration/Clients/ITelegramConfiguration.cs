namespace Bot.GetByLink.Common.Interfaces.Configuration.Clients;

/// <summary>
///     Telegram client configuration interface.
/// </summary>
public interface ITelegramConfiguration
{
    /// <summary>
    ///     Gets access token.
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    ///     Gets chat for logging issues (database not used).
    /// </summary>
    public string ChatIdLog { get; init; }

    /// <summary>
    ///     Gets mediaGroup send delay in milliseconds.
    /// </summary>
    public int DelaySendingMediaGroupMilliseconds { get; init; }

    /// <summary>
    ///     Gets max size in mb for one photo in message.
    /// </summary>
    public double MaxSizeMbPhoto { get; init; }

    /// <summary>
    ///     Gets max size in mb for one video in message.
    /// </summary>
    public double MaxSizeMbVideo { get; init; }

    /// <summary>
    ///     Gets max text lenght for caption first  media.
    /// </summary>
    public int MaxTextLenghtFirstMedia { get; init; }

    /// <summary>
    ///     Gets max text lenght for message.
    /// </summary>
    public int MaxTextLenghtMessage { get; init; }

    /// <summary>
    ///     Gets max text lenght for message.
    /// </summary>
    public int MaxColMediaInMessage { get; init; }
}