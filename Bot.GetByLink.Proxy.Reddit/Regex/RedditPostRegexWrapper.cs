using Bot.GetByLink.Common.Infrastructure.Abstractions;

namespace Bot.GetByLink.Proxy.Reddit.Regex;

/// <summary>
///     Regular expression for Reddit main comment.
/// </summary>
internal class RedditPostRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RedditPostRegexWrapper" /> class.
    /// </summary>
    public RedditPostRegexWrapper()
        : base(@"https?:\/\/www.reddit.com\/r\/\S+/comments\/\S+")
    {
    }
}