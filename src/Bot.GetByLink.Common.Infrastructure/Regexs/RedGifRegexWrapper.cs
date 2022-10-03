using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for regif.
/// </summary>
/// https://redgif.com/*
public sealed class RedGifRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RedGifRegexWrapper" /> class.
    /// </summary>
    public RedGifRegexWrapper()
        : base(@"https?:\/\/(www.)?redgifs.com\/")
    {
    }
}