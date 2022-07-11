using Bot.GetByLink.Common.Infrastructure.Model;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Polling.Interfaces;

/// <summary>
///     Interafece for formating content for messages telegram.
/// </summary>
public interface IFormaterContent
{
    /// <summary>
    ///     Function get formatted content.
    /// </summary>
    /// <param name="responseContent">Proxy content.</param>
    /// <returns>Formatted content.</returns>
    public (List<string> Messages, List<IAlbumInputMedia> Artifacts) GetFormattedContent(
        ProxyResponseContent responseContent);
}