using Bot.GetByLink.Common.Abstractions.Proxy;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Proxy.Vk.Interfaces;
using Bot.GetByLink.Proxy.Vk.Model;
using Bot.GetByLink.Proxy.Vk.Model.Regexs;
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
    private readonly IContentReturnStrategy photoStrategy;
    private readonly IContentReturnStrategy albumStrategy;


    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyVK"/> class.
    /// </summary>
    /// <param name="configuration">Bot configuration.</param>
    /// <param name="loggerPhotoStrategy">Interface for logging PhotoStrategy.</param>
    /// <param name="loggerAlbumStrategy">Interface for logging AlbumStrategy.</param>
    public ProxyVK(
        IBotConfiguration? configuration,
        ILogger<PhotoStrategy> loggerPhotoStrategy,
        ILogger<AlbumStrategy> loggerAlbumStrategy)
        : base(new[] { PhotoStrategy.PhotoRegex, AlbumStrategy.AlbumRegex })
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(loggerPhotoStrategy);
        ArgumentNullException.ThrowIfNull(loggerAlbumStrategy);

        var isParse = ulong.TryParse(configuration.Proxy.Vk.AppId, out var applicationId);
        var apiAuthParams = new ApiAuthParams
        {
            AccessToken = configuration.Proxy.Vk.ServiceAccessKey,
            ClientSecret = configuration.Proxy.Vk.SecureKey
        };
        if (isParse) apiAuthParams.ApplicationId = applicationId;

        api.Authorize(apiAuthParams);
        var idResourceRegexWrapper = new IdResourceRegexWrapper();
        photoStrategy = new PhotoStrategy(api, idResourceRegexWrapper, loggerPhotoStrategy);
        albumStrategy = new AlbumStrategy(api, idResourceRegexWrapper, loggerAlbumStrategy, photoStrategy);
    }

    /// <summary>
    ///     Method for getting the content of the post by url to the post.
    /// </summary>
    /// <param name="url">Url to post.</param>
    /// <returns>An object with text and links to pictures and videos present in the post.</returns>
    public override Task<IProxyContent?> GetContentUrlAsync(string url)
    {
        return url switch
        {
            var value when PhotoStrategy.PhotoRegex.IsMatch(value) => photoStrategy.TryGetByUrlAsync(value),
            var value when AlbumStrategy.AlbumRegex.IsMatch(value) => albumStrategy.TryGetByUrlAsync(value),
            _ => Task.FromResult<IProxyContent?>(null),
        };
    }
}