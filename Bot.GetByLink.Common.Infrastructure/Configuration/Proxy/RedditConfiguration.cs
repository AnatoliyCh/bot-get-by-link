using System.Text.Json.Serialization;

namespace Bot.GetByLink.Common.Infrastructure.Configuration.Proxy;

/// <summary>
///     Reddit proxy configuration.
/// </summary>
public sealed class RedditConfiguration
{
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