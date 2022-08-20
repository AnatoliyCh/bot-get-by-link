using Bot.GetByLink.Common.Abstractions.Proxy;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Regexs;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Model;

namespace Bot.GetByLink.Proxy.Vk;

/// <summary>
///     VK API for getting post content by URL.
/// </summary>
public sealed class ProxyVK : ProxyService
{
    private readonly VkApi api = new();
    private readonly WellApi wellApi;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProxyVK" /> class.
    /// </summary>
    /// <param name="configuration">Bot configuration.</param>
    /// <param name="loggerWellApi">Interface for logging.</param>
    public ProxyVK(IBotConfiguration? configuration, ILogger<WellApi> loggerWellApi)
        : base(new[] { new WallRegexWrapper() })
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(loggerWellApi);

        var isParse = ulong.TryParse(configuration.Proxy.Vk.AppId, out var applicationId);
        var apiAuthParams = new ApiAuthParams
        {
            AccessToken = configuration.Proxy.Vk.ServiceAccessKey,
            ClientSecret = configuration.Proxy.Vk.SecureKey
        };
        if (isParse) apiAuthParams.ApplicationId = applicationId;

        api.Authorize(apiAuthParams);
        wellApi = new WellApi(api, loggerWellApi);
    }

    /// <summary>
    ///     Method for getting the content of the post by url to the post.
    /// </summary>
    /// <param name="url">Url to post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override async Task<IProxyContent?> GetContentUrlAsync(string url)
    {
        var id = wellApi.WallPostIdRegex.Match(url)?.Value;
        var content = await wellApi.TryGetByIdAsync(id);
        return content;
    }
}