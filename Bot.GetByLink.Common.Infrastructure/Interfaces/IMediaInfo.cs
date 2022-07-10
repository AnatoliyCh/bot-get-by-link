namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

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
    public long SizeMedia { get; set; }

    /// <summary>
    ///     Function for set imfo media on url media.
    /// </summary>
    /// <param name="url">Url media</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public Task SetMediaInfoAsync(string url);
}