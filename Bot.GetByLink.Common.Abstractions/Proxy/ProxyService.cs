using System.Linq;
using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Common.Abstractions.Proxy;

/// <summary>
///     Base abstract proxy class.
/// </summary>
public abstract class ProxyService : IProxyService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyService" /> class.
    /// </summary>
    /// <param name="regexUrl">Regex url for proxy.</param>
    protected ProxyService(IRegexWrapper[] regexUrl)
    {
        RegexUrl = regexUrl;
    }

    /// <summary>
    ///     Gets regex url for proxy.
    /// </summary>
    public IRegexWrapper[] RegexUrl { get; }

    /// <summary>
    ///     Get match url with regex for this proxy.
    /// </summary>
    /// <param name="url">Url for check.</param>
    /// <returns>Did pass url.</returns>
    public bool IsMatch(string url)
    {
        return RegexUrl.Any(regex => regex.IsMatch(url));
    }

    /// <summary>
    ///     Method for getting the content of the post by url to the post.
    /// </summary>
    /// <param name="url">Url to post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public abstract Task<IProxyContent?> GetContentUrlAsync(string url);
}