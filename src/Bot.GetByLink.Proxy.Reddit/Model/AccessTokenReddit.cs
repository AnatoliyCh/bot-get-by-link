using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model;

/// <summary>
///     Reddit access token class.
/// </summary>
public sealed class AccessTokenReddit
{
    /// <summary>
    ///     Gets  access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; } = string.Empty;

    /// <summary>
    ///     Gets  type of token.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; } = string.Empty;

    /// <summary>
    ///     Gets  how many seconds the token lives.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; init; } = 0;

    /// <summary>
    ///     Gets  which scope is available for this token (* - all).
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; init; } = string.Empty;
}