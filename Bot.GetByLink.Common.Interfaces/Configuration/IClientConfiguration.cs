using Bot.GetByLink.Common.Interfaces.Configuration.Clients;

namespace Bot.GetByLink.Common.Interfaces.Configuration;

/// <summary>
///     Client configuration interface.
/// </summary>
public interface IClientConfiguration
{
    /// <summary>
    ///     Gets Telegram client configuration.
    /// </summary>
    public ITelegramConfiguration Telegram { get; init; }
}
