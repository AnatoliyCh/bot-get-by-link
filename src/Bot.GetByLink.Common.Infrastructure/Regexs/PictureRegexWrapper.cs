﻿using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for picture.
/// </summary>
/// https://*.jpg
/// https://*.jpeg
/// https://*.png
public sealed class PictureRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PictureRegexWrapper" /> class.
    /// </summary>
    public PictureRegexWrapper()
        : base(@"https?://\S+(?:jpg|jpeg|png)")
    {
    }
}