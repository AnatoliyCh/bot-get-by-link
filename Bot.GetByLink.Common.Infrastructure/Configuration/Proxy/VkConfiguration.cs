using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

/// <summary>
///     Vk proxy configuration.
/// </summary>
public sealed class VkConfiguration : IVkConfiguration
{
    /// <summary>
    ///     Gets a value indicating whether whether to start a proxy.
    /// </summary>
    [JsonPropertyName("Run")]
    public bool Run { get; init; } = false;

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