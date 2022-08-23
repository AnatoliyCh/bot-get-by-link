using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;
using System.Text.Json.Serialization;

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

    /// <summary>
    ///     Gets the number of objects from the album per step.
    /// </summary>
    [JsonPropertyName("StepObjectsInAlbum")]
    public int StepObjectsInAlbum { get; init; } = 50;
}