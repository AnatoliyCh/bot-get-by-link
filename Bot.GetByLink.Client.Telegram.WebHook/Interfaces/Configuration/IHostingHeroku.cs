namespace Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

/// <summary>
/// Heroku hosting settings.
/// </summary>
public interface IHostingHeroku
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
