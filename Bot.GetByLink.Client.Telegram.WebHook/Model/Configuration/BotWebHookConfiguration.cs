using Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;
using Bot.GetByLink.Common.Infrastructure.Configuration;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Client.Telegram.WebHook.Model.Configuration;

/// <summary>
///     Bot сonfiguration class.
///     Hosting: Heroku.
/// </summary>
public sealed class BotWebHookConfiguration : BotConfiguration, IBotWebHookConfiguration
{
    /// <summary>
    ///    Gets hosting settings.
    /// </summary>
    [JsonPropertyName("Server")]
    public IDeployConfiguration Server { get; init; } = new DeployConfiguration();
}
