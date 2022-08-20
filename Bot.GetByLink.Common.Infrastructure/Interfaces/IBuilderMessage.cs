namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     The interface of an object that build messages.
/// </summary>
/// <typeparam name="TFrom">Type content for build messages.</typeparam>
/// <typeparam name="TReturtBuilder">Type return builder message.</typeparam>
/// <typeparam name="TResult">Type ready messages.</typeparam>
public interface IBuilderMessage<in TFrom, out TReturtBuilder, out TResult>
    where TFrom : class, IProxyContent
    where TReturtBuilder : class, IBuilderMessage<TFrom, TReturtBuilder, TResult>
{
    /// <summary>
    ///     From conter build message.
    /// </summary>
    /// <param name="proxyContent">Content.</param>
    /// <returns>This builder.</returns>
    public TReturtBuilder From(TFrom proxyContent);

    /// <summary>
    ///     Add url in builder.
    /// </summary>
    /// <param name="url">Url.</param>
    /// <returns>This builder.</returns>
    public TReturtBuilder AddUrl(string url);

    /// <summary>
    ///     Build messages.
    /// </summary>
    /// <returns>Ready messages.</returns>
    public TResult Build();
}