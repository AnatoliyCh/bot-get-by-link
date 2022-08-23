using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for Document.
/// </summary>
/// https://vk.com/doc000000000_000000000
/// https://vk.com/doc000000000_000000000?hash=wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
internal sealed class UrlDocRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlDocRegexWrapper" /> class.
    /// </summary>
    public UrlDocRegexWrapper()
        : base(@"https?:\/\/vk\.com\/doc?\d+_\d+")
    {
    }
}