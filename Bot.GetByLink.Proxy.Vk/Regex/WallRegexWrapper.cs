using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Regex;

/// <summary>
///     Regular expression for Wall VK.
/// </summary>
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