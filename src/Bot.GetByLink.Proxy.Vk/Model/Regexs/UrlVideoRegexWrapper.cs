using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for Video.
/// </summary>
/// https://vk.com/wall-000000000_000000000?z=video-000000000_000000000%2F000000000_000000000%2Fpl_post_-000000000_000000000
/// https://vk.com/video-000000000_000000000?list=000000000
internal sealed class UrlVideoRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlVideoRegexWrapper" /> class.
    /// </summary>
    public UrlVideoRegexWrapper()
        : base(@"https?:\/\/vk\.com\/(([^\?]+\?z=)|())video-?\d+_\d+")
    {
    }
}