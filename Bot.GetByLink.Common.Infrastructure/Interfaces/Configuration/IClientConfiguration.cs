using Bot.GetByLink.Common.Infrastructure.Configuration.Clients;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces.Configuration;

/// <summary>
///     Client configuration interface.
/// </summary>
public interface IClientConfiguration
{
    /// <summary>
    ///     Gets Telegram client configuration.
    /// </summary>
    public ClientConfiguration Telegram { get; init; }

    /// <summary>
    ///     Gets Discord client configuration.
    /// </summary>
    public ClientConfiguration Discord { get; init; }
}
