using Bot.GetByLink.Common.Interfaces.Configuration;

namespace Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

/// <summary>
///     Bot сonfiguration interface.
/// </summary>
public interface IBotWebHookConfiguration : IBotConfiguration
{
    /// <summary>
    ///    Gets hosting settings.
    /// </summary>
    public IDeployConfiguration Server { get; init; }
}
