namespace Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

/// <summary>
///     Reddit proxy setup interface.
/// </summary>
public interface IRedditConfiguration : IProxyConfiguration
{
    /// <summary>
    ///     Gets application Id.
    /// </summary>
    public string AppId { get; init; }

    /// <summary>
    ///     Gets secret key.
    /// </summary>
    public string Secret { get; init; }

    /// <summary>
    ///     Gets sub services.
    /// </summary>
    public IRedditSubServicesConfiguration? SubServices { get; init; }
}