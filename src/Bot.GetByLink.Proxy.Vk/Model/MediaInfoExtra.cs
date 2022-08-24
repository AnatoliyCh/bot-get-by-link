using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
///     Class for info media and additional information.
/// </summary>
/// <param name="Url">Url media.</param>
/// <param name="Size">Size media.</param>
/// <param name="Type">Type media.</param>
/// <param name="Title">Title media.</param>
/// <param name="Description">Description media.</param>
public sealed record MediaInfoExtra
    (string Url, double Size, MediaType Type, string Title, string Description) : IMediaInfo;