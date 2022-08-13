namespace Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

/// <summary>
///     Bot сonfiguration interface.
/// </summary>
public interface IBotConfiguration : GetByLink.Common.Interfaces.Configuration.IBotConfiguration
{
    /// <summary>
    ///    Gets hosting settings.
    /// </summary>
    public IHostingHeroku Hosting { get; init; }
}
