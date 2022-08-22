using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for photo.
/// </summary>
/// https://vk.com/feed?z=photo0_000000000%2Falbum-0000000000_000000000%2Frev
/// https://vk.com/feed?z=photo6490_000000000%2Falbum-0000000000_000000000%2Frev
/// https://vk.com/feed?z=photo-0_000000000%2Falbum-0000000000_000000000%2Frev
/// https://vk.com/anyWords?z=photo-0000000000_000000000%2Falbum-0000000000_000000000%2Frev
internal sealed class UrlPhotoRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlPhotoRegexWrapper" /> class.
    /// </summary>
    public UrlPhotoRegexWrapper()
        : base(@"https?:\/\/vk\.com\/[^\?]+\?z=photo-?\d+_\d+")
    {
    }
}