using Newtonsoft.Json;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
/// Reddit access token class.
/// </summary>
public class AccessTokenReddit
{
    /// <summary>
    /// Gets or sets Access token.
    /// </summary>
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets Type of token.
    /// </summary>
    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// Gets or sets How many seconds the token lives.
    /// </summary>
    [JsonProperty("expires_in")]
    public string? ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets Which scope is available for this token (* - all).
    /// </summary>
    [JsonProperty("scope")]
    public string? Scope { get; set; }
}