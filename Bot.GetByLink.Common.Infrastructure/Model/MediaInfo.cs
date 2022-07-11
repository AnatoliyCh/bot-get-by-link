using Bot.GetByLink.Common.Infrastructure.Enums;

namespace Bot.GetByLink.Common.Infrastructure.Model;

/// <summary>
///     Class for info media.
/// </summary>
/// <param name="Url">Url media.</param>
/// <param name="Size">Size media.</param>
/// <param name="Type">Type media.</param>
public sealed record MediaInfo(string Url, long Size, MediaType Type);