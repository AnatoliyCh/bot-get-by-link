using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Model;

/// <summary>
///     Сlass for returned object from proxy.
/// </summary>
public sealed record ProxyResponseContent(string Text, IMediaInfo[] UrlPicture, IMediaInfo[] UrlVideo) : IProxyContent;