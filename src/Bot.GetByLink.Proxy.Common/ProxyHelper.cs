namespace Bot.GetByLink.Proxy.Common;

/// <summary>
///     Class general methods for proxy.
/// </summary>
public static class ProxyHelper
{
    private const long OneKb = 1024;
    private const long OneMb = OneKb * 1024;

    /// <summary>
    ///     Function for get size content from url.
    /// </summary>
    /// <param name="url">Url for get size content.</param>
    /// <returns>Size content.</returns>
    public static async Task<long> GetSizeContentUrlAsync(string url)
    {
        HttpClient client = new();
        long result = -1;
        client.BaseAddress = new Uri(url);
        var request = new HttpRequestMessage(HttpMethod.Head, string.Empty);
        var message = await client.SendAsync(request);
        if (long.TryParse(
                message.Content.Headers.FirstOrDefault(h => h.Key.Equals("Content-Length")).Value?.FirstOrDefault(),
                out var contentLength)) result = contentLength == 0 ? contentLength : contentLength / OneMb;

        return result;
    }
}