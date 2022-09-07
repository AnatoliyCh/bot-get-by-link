using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Common.Infrastructure.Proxy;

/// <summary>
///     Class for info media.
/// </summary>
/// <param name="Url">Url media.</param>
/// <param name="Size">Size media.</param>
/// <param name="Type">Type media.</param>
/// <param name="Width">Width media.</param>
/// <param name="Height">Height media.</param>
public sealed record MediaInfo(string Url, double Size, MediaType Type, int? Width = 0, int? Height = 0) : IMediaInfo;