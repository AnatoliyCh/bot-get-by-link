﻿namespace Bot.GetByLink.Common.Interfaces.Configuration.Proxy;

/// <summary>
///     Imgur proxy setup interface.
/// </summary>
public interface IImgurConfiguration : IProxyConfiguration
{
    /// <summary>
    ///     Gets application Id.
    /// </summary>
    public string AppId { get; init; }

    /// <summary>
    ///     Gets secret key.
    /// </summary>
    public string Secret { get; init; }
}