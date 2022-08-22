using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for Wall VK.
/// </summary>
/// https://vk.com/wall-000000000_0
/// https://vk.com/feed?w=wall-000000000_00
/// http://vk.com/wall000000000_000
internal sealed class WallRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WallRegexWrapper" /> class.
    /// </summary>
    public WallRegexWrapper()
        : base(@"https?:\/\/vk\.com\/(wall-\d+_\d+$|feed\?w=wall-\d+_\d+$|wall\d+_\d+$)")
    {
    }
}