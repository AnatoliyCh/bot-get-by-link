using Bot.GetByLink.Common.Infrastructure.Model;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     Interafece for formating content for messages telegram.
/// </summary>
public interface IFormatterContent
{
    /// <summary>
    ///     Function get formatted content.
    /// </summary>
    /// <param name="responseContent">Proxy content.</param>
    /// <returns>Formatted content.</returns>
    public (IEnumerable<string> Messages, IEnumerable<IAlbumInputMedia> Artifacts) GetFormattedContent(
        ProxyResponseContent responseContent);
}