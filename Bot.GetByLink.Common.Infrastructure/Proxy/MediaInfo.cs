using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Proxy;

/// <summary>
///     Class for info media.
/// </summary>
/// <param name="Url">Url media.</param>
/// <param name="Size">Size media.</param>
/// <param name="Type">Type media.</param>
public sealed record MediaInfo(string Url, double Size, MediaType Type) : IMediaInfo;