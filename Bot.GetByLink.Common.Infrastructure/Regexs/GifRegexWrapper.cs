using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for gif.
/// </summary>
public class GifRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GifRegexWrapper" /> class.
    /// </summary>
    public GifRegexWrapper()
        : base(@".(GIF|gif)v?$")
    {
    }
}