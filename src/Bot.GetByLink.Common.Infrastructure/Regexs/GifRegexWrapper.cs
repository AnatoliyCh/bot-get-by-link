using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for gif.
/// </summary>
/// *.gif
/// *.GIF
/// *.gifv
/// *.GIFv
public sealed class GifRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GifRegexWrapper" /> class.
    /// </summary>
    public GifRegexWrapper()
        : base(@".(GIF|gif)v?$")
    {
    }
}