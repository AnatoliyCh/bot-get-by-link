using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Reddit.Regexs;

/// <summary>
///     Regular expression for Reddit main comment.
/// </summary>
/// https://www.reddit.com/r/*/(comments|gallery)/*
public sealed class RedditPostRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RedditPostRegexWrapper" /> class.
    /// </summary>
    public RedditPostRegexWrapper()
        : base(@"https?:\/\/www.reddit.com[\/]?([\S]+?)[\/]?(comments|gallery)\/\S+")
    {
    }
}