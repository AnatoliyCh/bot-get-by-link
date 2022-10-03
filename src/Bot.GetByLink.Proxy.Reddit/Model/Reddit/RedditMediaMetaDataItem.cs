using System.Text.Json.Serialization;

namespace Bot.GetByLink.Proxy.Reddit.Model.Reddit;

/// <summary>
///     Class for media metadata item.
/// </summary>
public sealed class RedditMediaMetaDataItem
{
    /// <summary>
    ///     Gets status.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; } = string.Empty;

    /// <summary>
    ///     Gets type element.
    /// </summary>
    [JsonPropertyName("e")]
    public string? ElementType { get; init; } = string.Empty;

    /// <summary>
    ///     Gets extension element.
    /// </summary>
    [JsonPropertyName("m")]
    public string? ElementExtension { get; init; } = string.Empty;

    /// <summary>
    ///     Gets preview elements.
    /// </summary>
    [JsonPropertyName("p")]
    public IList<RedditMediaMetaDataItemSource>? PreviewElements { get; init; } = null;

    /// <summary>
    ///     Gets source element.
    /// </summary>
    [JsonPropertyName("s")]
    public RedditMediaMetaDataItemSource? SourceElements { get; init; } = null;

    /// <summary>
    ///     Gets id element.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; } = string.Empty;
}