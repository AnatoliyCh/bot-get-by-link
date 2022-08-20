using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Proxy;

/// <summary>
///     Сlass for returned object from proxy.
/// </summary>
public sealed record ProxyResponseContent(string Text, string? Header = null,
    IEnumerable<IMediaInfo>? UrlPicture = null,
    IEnumerable<IMediaInfo>? UrlVideo = null) : IProxyContent;