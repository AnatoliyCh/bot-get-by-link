using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Bot.GetByLink.Common.Infrastructure.Model;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

/// <summary>
///     Base abstract proxy class.
/// </summary>
public abstract class ProxyService : IProxyService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyService" /> class.
    /// </summary>
    /// <param name="regexUrl">Regex url for proxy.</param>
    protected ProxyService(string[] regexUrl)
    {
        RegexUrl = regexUrl;
    }

    /// <summary>
    ///     Gets regex url for proxy.
    /// </summary>
    public string[] RegexUrl { get; }

    /// <summary>
    ///     Get match url with regex for this proxy.
    /// </summary>
    /// <param name="url">Url for check.</param>
    /// <returns>Did pass url.</returns>
    public bool IsMatch(string url)
    {
        return RegexUrl.Any(x => Regex.IsMatch(url, x));
    }

    /// <summary>
    ///     Method for getting the content of the post by url to the post.
    /// </summary>
    /// <param name="url">Url to post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public abstract Task<ProxyResponseContent> GetContentUrlAsync(string url);
}