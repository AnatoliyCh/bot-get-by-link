using Bot.GetByLink.Common.Infrastructure.Interfaces;
using System.Text.RegularExpressions;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions
{
    /// <summary>
    /// Base abstract proxy class.
    /// </summary>
    public abstract class ProxyService : IProxyService
    {
        /// <summary>
        /// Gets regex url for proxy.
        /// </summary>
        public string[] RegexUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyService"/> class.
        /// </summary>
        /// <param name="regexUrl">Regex url for proxy.</param>
        protected ProxyService(string[] regexUrl)
        {
            RegexUrl = regexUrl;
        }

        /// <summary>
        /// Get match url with regex for this proxy.
        /// </summary>
        /// <param name="url">Url for check.</param>
        /// <returns>Did pass url.</returns>
        public bool IsMatch(string url)
        {
            return RegexUrl.Any(x => Regex.IsMatch(url, x));
        }

        /// <summary>
        /// Метод для получения контента поста по url на пост.
        /// </summary>
        /// <param name="url">Url на пост.</param>
        /// <returns>Объект с текстом и ссылками на картинки и видео присутствующие в посте.</returns>
        public abstract Task<TelegramMessage> GetContentUrl(string url);
    }
}
