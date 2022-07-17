using Bot.GetByLink.Common.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Client.Telegram.Common.Model
{
    /// <summary>
    ///     Regular expression for picture.
    /// </summary>
    public sealed class PictureRegexWrapper : RegexWrapper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PictureRegexWrapper" /> class.
        /// </summary>
        public PictureRegexWrapper() 
            : base(@"https?://\S+(?:jpg|jpeg|png)")
        {
        }
    }
}
