using System.Text.Json.Serialization;
using Bot.GetByLink.Client.Telegram.WebHook.Interfaces.Configuration;

namespace Bot.GetByLink.Client.Telegram.WebHook.Model.Configuration;

/// <summary>
///     Heroku hosting settings.
/// </summary>
public sealed class DeployConfiguration : IDeployConfiguration
{
    /// <summary>
    ///     Gets the url.
    /// </summary>
    [JsonPropertyName("HOST")]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    ///     Gets the port.
    /// </summary>
    [JsonPropertyName("PORT")]
    public string Port { get; init; } = string.Empty;
}