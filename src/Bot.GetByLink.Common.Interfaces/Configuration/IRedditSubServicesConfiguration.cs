using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

namespace Bot.GetByLink.Common.Interfaces.Configuration;

/// <summary>
///     Reddit sub services proxy setup interface.
/// </summary>
public interface IRedditSubServicesConfiguration
{
    /// <summary>
    ///     Gets imgur.
    /// </summary>
    public IImgurConfiguration? Imgur { get; init; }
}