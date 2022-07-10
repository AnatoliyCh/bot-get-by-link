using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure;

/// <summary>
///     Class for info media.
/// </summary>
public class MediaInfo : IMediaInfo
{
    private const long OneKb = 1024;
    private const long OneMb = OneKb * 1024;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MediaInfo" /> class.
    /// </summary>
    public MediaInfo()
    {
        SizeMedia = -1;
        UrlMedia = string.Empty;
    }

    /// <summary>
    ///     Gets or sets for url media.
    /// </summary>
    public string UrlMedia { get; set; }

    /// <summary>
    ///     Gets or sets for size in mb on url media.
    /// </summary>
    public double SizeMedia { get; set; }

    /// <summary>
    ///     Function for set imfo media on url media.
    /// </summary>
    /// <param name="url">Url media</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task SetMediaInfoAsync(string url)
    {
        UrlMedia = url;
        SizeMedia = await GetSizeContentUrlAsync(url);
    }

    /// <summary>
    ///     Function for get size content from url.
    /// </summary>
    /// <param name="url">Url for get size content.</param>
    /// <returns>Size content.</returns>
    private static async Task<long> GetSizeContentUrlAsync(string url)
    {
        HttpClient client = new();
        long result = -1;
        client.BaseAddress = new Uri(url);
        var request = new HttpRequestMessage(HttpMethod.Head, string.Empty);
        var message = await client.SendAsync(request);
        if (long.TryParse(
                message.Content.Headers.FirstOrDefault(h => h.Key.Equals("Content-Length")).Value.FirstOrDefault(),
                out var contentLength)) result = contentLength == 0 ? contentLength : contentLength / OneMb;

        return result;
    }
}