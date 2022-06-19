using Newtonsoft.Json;

namespace Bot.GetByLink.Proxy.Reddit;

/// <summary>
/// Класс токена доступа у Реддита.
/// </summary>
public class AccessTokenReddit
{
    /// <summary>
    /// Gets or sets Токен доступа.
    /// </summary>
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets Тип токена.
    /// </summary>
    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// Gets or sets Сколько секунд живёт токен.
    /// </summary>
    [JsonProperty("expires_in")]
    public string? ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets Какой скоуп доступен для данного токена (* - все).
    /// </summary>
    [JsonProperty("scope")]
    public string? Scope { get; set; }
}