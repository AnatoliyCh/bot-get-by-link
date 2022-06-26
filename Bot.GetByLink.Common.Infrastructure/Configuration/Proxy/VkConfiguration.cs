using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

/// <summary>
///     Vk proxy configuration.
/// </summary>
public sealed class VkConfiguration
{
    /// <summary>
    ///     Gets application Id.
    /// </summary>
    [JsonPropertyName("AppId")]
    public string AppId { get; init; } = string.Empty;

    /// <summary>
    ///     Gets secure key.
    /// </summary>
    [JsonPropertyName("SecureKey")]
    public string SecureKey { get; init; } = string.Empty;

    /// <summary>
    ///     Gets service access key.
    /// </summary>
    [JsonPropertyName("ServiceAccessKey")]
    public string ServiceAccessKey { get; init; } = string.Empty;
}