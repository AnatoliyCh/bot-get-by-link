using Bot.GetByLink.Common.Infrastructure.Enums;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Interface for info media.
/// </summary>
public interface IMediaInfo
{
    /// <summary>
    ///     Gets for url media.
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    ///     Gets for size in mb on url media.
    /// </summary>
    public double Size { get; init; }

    /// <summary>
    ///     Gets type media.
    /// </summary>
    public MediaType Type { get; init; }
}