using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     An object containing all the data to send a message.
/// </summary>
/// <param name="ChatId">Сhat ID to send the message.</param>
/// <param name="Text">Message text.</param>
/// <param name="Artifacts">Collection of Attached Artifacts.</param>
/// <param name="ParseMode">Text parsing mode.</param>
public sealed record Message
(ChatId ChatId, IEnumerable<string> Text, IEnumerable<IEnumerable<IAlbumInputMedia>>? Artifacts = null,
    ParseMode? ParseMode = null) : IMessageContext;