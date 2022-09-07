using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Reddit.Regexs;

/// <summary>
///     Regular expression for Reddit main comment.
/// </summary>
/// https://www.reddit.com/r/*/gallery/*
public sealed class RedditGalleryRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RedditGalleryRegexWrapper" /> class.
    /// </summary>
    public RedditGalleryRegexWrapper()
        : base(@"https?:\/\/www.reddit.com\/gallery\/\S+")
    {
    }
}