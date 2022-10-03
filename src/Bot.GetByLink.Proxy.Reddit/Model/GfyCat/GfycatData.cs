using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.GfyCat;

/// <summary>
///     Class for gfycat data api.
/// </summary>
public sealed class GfycatData
{
    /// <summary>
    ///     Gets gfy item.
    /// </summary>
    [JsonPropertyName("gfyItem")]
    public GfyItem? GfyItem { get; init; } = null;

    /// <summary>
    ///     Gets unknow fields.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? UnknowFields { get; init; } = null;
}