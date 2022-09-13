using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for imgur.
/// </summary>
/// https://imgur.com/*
public sealed class ImgurRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImgurRegexWrapper" /> class.
    /// </summary>
    public ImgurRegexWrapper()
        : base(@"https?:\/\/(www.)?(i.)?imgur.com\/")
    {
    }
}