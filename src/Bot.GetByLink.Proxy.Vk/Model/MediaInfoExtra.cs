﻿using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces.Proxy;

namespace Bot.GetByLink.Proxy.Vk.Model;

/// <summary>
///     Class for info media and additional information.
/// </summary>
/// <param name="Url">Url media.</param>
/// <param name="Width">Width media.</param>
/// <param name="Height">Height media.</param>
/// <param name="Size">Size media.</param>
/// <param name="Type">Type media.</param>
/// <param name="Title">Title media.</param>
/// <param name="Description">Description media.</param>
/// <param name="IsArtifact">Whether to add to the media group.</param>
public sealed record MediaInfoExtra
(string Url, double Size, MediaType Type, int? Width = 0, int? Height = 0, string? Title = null,
    string? Description = null,
    bool IsArtifact = false) : IMediaInfo;