namespace Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

/// <summary>
///     Deployment settings.
/// </summary>
public interface IDeployConfiguration
{
    /// <summary>
    ///     Gets the url.
    /// </summary>
    public string Url { get; }

    /// <summary>
    ///     Gets the port.
    /// </summary>
    public string Port { get; }
}