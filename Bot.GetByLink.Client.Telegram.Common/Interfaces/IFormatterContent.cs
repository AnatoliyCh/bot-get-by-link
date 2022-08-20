using Bot.GetByLink.Common.Interfaces.Configuration.Clients;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     Interafece for formating content for messages telegram.
/// </summary>
public interface IFormatterContent
{
    /// <summary>
    ///     Function get messages from proxy content.
    /// </summary>
    /// <param name="proxyContent">Proxy content.</param>
    /// <param name="setHeader">Set header in messages.</param>
    /// <param name="url">Url post in messages.</param>
    /// <param name="configuration">Configuration telegram.</param>
    /// <returns>List messages.</returns>
    public (IEnumerable<string> Messages, IEnumerable<IEnumerable<IAlbumInputMedia>> Artifacts) GetListMessages(
        IProxyContent proxyContent, bool setHeader, string url, ITelegramConfiguration configuration);
}