using System.Text.Json.Serialization;
using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

/// <summary>
///     Reddit proxy configuration.
/// </summary>
public sealed class RedditConfiguration : IRedditConfiguration
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
    ///     Gets secret key.
    /// </summary>
    [JsonPropertyName("Secret")]
    public string Secret { get; init; } = string.Empty;
}