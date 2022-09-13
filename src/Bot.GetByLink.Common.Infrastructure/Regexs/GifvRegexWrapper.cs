using Bot.GetByLink.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Common.Infrastructure.Regexs
{
    /// <summary>
    ///     Regular expression for gifv.
    /// </summary>
    /// *.gifv
    /// *.GIFV
    public sealed class GifvRegexWrapper : RegexWrapper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GifvRegexWrapper" /> class.
        /// </summary>
        public GifvRegexWrapper()
            : base(@".(GIFV|gifv)$")
        {
        }
    }
}
