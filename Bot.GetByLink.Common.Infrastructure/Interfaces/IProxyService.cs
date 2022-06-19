﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces
{
    /// <summary>
    /// Proxy interface.
    /// </summary>
    public interface IProxyService
    {
        /// <summary>
        /// Gets regex url for proxy.
        /// </summary>
        public string[] RegexUrl { get; }

        /// <summary>
        /// Get match url with regex for this proxy.
        /// </summary>
        /// <param name="url">Url for check.</param>
        /// <returns>Did pass url.</returns>
        public bool IsMatch(string url);

        /// <summary>
        /// Метод для получения контента поста по url на пост.
        /// </summary>
        /// <param name="url">Url на пост.</param>
        /// <returns>Объект с текстом и ссылками на картинки и видео присутствующие в посте.</returns>
        public abstract Task<TelegramMessage> GetContentUrl(string url);
    }
}