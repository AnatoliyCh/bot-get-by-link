using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for post reddit.
/// </summary>
public sealed class RedditPost
{
    /// <summary>
    ///     Gets data post reddit.
    /// </summary>
    [JsonPropertyName("data")]
    public RedditPostData? Data { get; init; } = null;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}