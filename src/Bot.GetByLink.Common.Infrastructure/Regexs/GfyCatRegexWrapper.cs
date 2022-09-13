using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Common.Infrastructure.Regexs
{
    /// <summary>
    ///     Regular expression for gfycat.
    /// </summary>
    /// https://gfycat.com/*
    public sealed class GfyCatRegexWrapper : RegexWrapper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GfyCatRegexWrapper" /> class.
        /// </summary>
        public GfyCatRegexWrapper()
            : base(@"https?:\/\/gfycat.com\/")
        {
        }
    }
}
