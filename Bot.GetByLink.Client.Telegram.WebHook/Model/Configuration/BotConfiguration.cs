using Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

namespace Bot.GetByLink.Client.Telegram.WebHook.Model.Configuration;

/// <summary>
///     Bot сonfiguration class.
///     Hosting: Heroku.
/// </summary>
public class BotConfiguration : GetByLink.Common.Infrastructure.Configuration.BotConfiguration, IBotConfiguration
{
    /// <summary>
    ///    Gets hosting settings.
    /// </summary>
    public IHostingHeroku Hosting { get; init; } = new HostingHeroku();
}
