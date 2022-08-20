using Bot.GetByLink.Common.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Common.Infrastructure.Model
{
    /// <summary>
    ///     Regular expression for gif.
    /// </summary>
    public class GifRegexWrapper : RegexWrapper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GifRegexWrapper" /> class.
        /// </summary>
        public GifRegexWrapper()
            : base(@".(GIF|gif)v?$")
        {
        }
    }
}
