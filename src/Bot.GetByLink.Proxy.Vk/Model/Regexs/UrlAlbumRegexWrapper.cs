using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for Album.
///     https://vk.com/ and (?=.*) ?z=album-000000000_000000000.
/// </summary>
/// https://vk.com/wall-000000000_000?z=album-000000000_000000000
/// https://vk.com/anyWords?z=album-000000000_000000000
// https://vk.com/wall-000000000_000?w=wall-000000000_000&z=album-000000000_000000000
internal sealed class UrlAlbumRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlAlbumRegexWrapper" /> class.
    /// </summary>
    public UrlAlbumRegexWrapper()
        : base(@"(&|\?)z=album-?\d+_\d+$")
    {
    }
}