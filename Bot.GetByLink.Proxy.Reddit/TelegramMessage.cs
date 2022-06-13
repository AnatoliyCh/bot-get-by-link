using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Proxy.Reddit
{
    /// <summary>
    /// Временный класс для возвратного сообщения.
    /// </summary>
    public class TelegramMessage
    {
        /// <summary>
        /// Gets or sets текст поста.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets картики в посте.
        /// </summary>
        public string? UrlPicture { get; set; }

        /// <summary>
        /// Gets or sets видео в посте.
        /// </summary>
        public string? UrlVideo { get; set; }
    }
}
