using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for Wall VK.
/// </summary>
/// https://vk.com/feed?w=wall-000000000_0
/// https://vk.com/wall-000000000_0
/// http://vk.com/wall-000000000_00
/// http://vk.com/wall000000000_000
/// https://vk.com/anyWords?w=wall-000000000_0
internal sealed class UrlWallRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlWallRegexWrapper" /> class.
    /// </summary>
    public UrlWallRegexWrapper()
        : base(@"https?:\/\/vk\.com\/([^\.]+)?wall-?\d+_\d+")
    {
    }
}