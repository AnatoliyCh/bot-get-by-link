using Bot.GetByLink.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Proxy.Reddit.Regexs
{
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
            : base(@"https?:\/\/www.reddit.com\/r\/\S+/gallery\/\S+")
        {
        }
    }
}
