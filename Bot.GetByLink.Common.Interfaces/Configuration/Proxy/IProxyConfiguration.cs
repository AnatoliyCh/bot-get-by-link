namespace Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

/// <summary>
/// Interface with basic fields for proxy.
/// </summary>
public interface IProxyConfiguration
{
    /// <summary>
    /// Gets a value indicating whether whether to start a proxy.
    /// </summary>
    public bool Run { get; }
}
