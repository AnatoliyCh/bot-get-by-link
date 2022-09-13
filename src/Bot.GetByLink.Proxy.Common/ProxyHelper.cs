using System.Net.Http.Headers;

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
                out var contentLength)) result = contentLength == 0 ? contentLength : GetMbFromByte(contentLength);

        return result;
    }

    /// <summary>
    /// Function for cast byte to megabyte.
    /// </summary>
    /// <param name="countByte">Count byte.</param>
    /// <returns>Count megabyte.</returns>
    public static long GetMbFromByte(long countByte) => countByte / OneMb;

    /// <summary>
    ///     Function for check has content from url.
    /// </summary>
    /// <param name="url">Url.</param>
    /// <returns>Has url content.</returns>
    public static async Task<bool> CheckContentUrl(string url)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(url);
        var productValue = new ProductInfoHeaderValue("BotGetByLink", "1.0");
        client.DefaultRequestHeaders.UserAgent.Add(productValue);
        var request = new HttpRequestMessage(HttpMethod.Head, string.Empty);
        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}