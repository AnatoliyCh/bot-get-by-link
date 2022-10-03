using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs;

/// <summary>
///     Regular expression for streamble.
/// </summary>
/// https://streamable.com/*
public sealed class StreamableRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StreamableRegexWrapper" /> class.
    /// </summary>
    public StreamableRegexWrapper()
        : base(@"https?:\/\/streamable.com\/")
    {
    }
}