namespace Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

/// <summary>
///     Vk proxy setup interface.
/// </summary>
public interface IVkConfiguration : IProxyConfiguration
{
    /// <summary>
    ///     Gets application Id.
    /// </summary>
    public string AppId { get; init; }

    /// <summary>
    ///     Gets secure key.
    /// </summary>
    public string SecureKey { get; init; }

    /// <summary>
    ///     Gets service access key.
    /// </summary>
    public string ServiceAccessKey { get; init; }
}
