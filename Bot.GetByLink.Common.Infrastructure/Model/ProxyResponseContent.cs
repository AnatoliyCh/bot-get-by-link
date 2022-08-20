using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Model;

/// <summary>
///     Сlass for returned object from proxy.
/// </summary>
public sealed record ProxyResponseContent(string Text, string? Header = null,
    IEnumerable<IMediaInfo>? UrlPicture = null,
    IEnumerable<IMediaInfo>? UrlVideo = null) : IProxyContent;