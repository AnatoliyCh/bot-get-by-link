﻿namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Interface for info media.
/// </summary>
public interface IMediaInfo
{
    /// <summary>
    ///     Gets or sets for url media.
    /// </summary>
    public string UrlMedia { get; set; }

    /// <summary>
    ///     Gets or sets for size in mb on url media.
    /// </summary>
    public double SizeMedia { get; set; }
}