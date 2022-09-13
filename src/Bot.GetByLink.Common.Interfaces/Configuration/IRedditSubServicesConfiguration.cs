using Bot.GetByLink.Common.Interfaces.Configuration.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Common.Interfaces.Configuration
{
    /// <summary>
    ///     Reddit sub services proxy setup interface.
    /// </summary>
    public interface IRedditSubServicesConfiguration
    {
        /// <summary>
        ///     Gets imgur.
        /// </summary>
        public IImgurConfiguration? Imgur { get; init; }
    }
}
