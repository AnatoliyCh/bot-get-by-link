using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Regex;

/// <summary>
///     Regular expression for Post Id by Wall VK.
/// </summary>
internal class WallPostIdRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WallPostIdRegexWrapper" /> class.
    /// </summary>
    public WallPostIdRegexWrapper()
        : base(@"-?\d+_\d+$")
    {
    }
}