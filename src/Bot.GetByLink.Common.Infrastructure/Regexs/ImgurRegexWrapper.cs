using Bot.GetByLink.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Common.Infrastructure.Regexs
{
    /// <summary>
    ///     
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
}
