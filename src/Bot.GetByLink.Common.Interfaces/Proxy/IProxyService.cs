﻿namespace Bot.GetByLink.Common.Interfaces.Proxy;

/// <summary>
///     Proxy interface.
/// </summary>
public interface IProxyService
{
    /// <summary>
    ///     Gets regex url for proxy.
    /// </summary>
    public IRegexWrapper[] RegexUrl { get; }

    /// <summary>
    ///     Get match url with regex for this proxy.
    /// </summary>
    /// <param name="url">Url for check.</param>
    /// <returns>Did pass url.</returns>
    public bool IsMatch(string url);

    /// <summary>
    ///     Method for getting the content of the post by url to the post.
    /// </summary>
    /// <param name="url">Url to post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public Task<IProxyContent?> GetContentUrlAsync(string url);
}