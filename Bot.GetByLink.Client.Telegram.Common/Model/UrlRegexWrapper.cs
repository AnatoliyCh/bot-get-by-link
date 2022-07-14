using Bot.GetByLink.Common.Infrastructure.Abstractions;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     Regular expression for URL.
/// </summary>
public sealed class UrlRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UrlRegexWrapper" /> class.
    /// </summary>
    public UrlRegexWrapper()
        : base(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")
    {
    }
}