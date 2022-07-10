using Bot.GetByLink.Common.Infrastructure;

namespace Bot.GetByLink.Client.Telegram.Polling.Interfaces;

/// <summary>
///     Interafece for formating content for messages telegram.
/// </summary>
public interface IFormaterContentTelegram
{
    /// <summary>
    ///     Function for set content.
    /// </summary>
    /// <param name="responseContent">Proxy content.</param>
    public void SetFormaterContent(ProxyResponseContent responseContent);
}