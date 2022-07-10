using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     An object containing all the data to send a message.
/// </summary>
/// <param name="ChatId">Сhat ID to send the message.</param>
/// <param name="Text">Message text.</param>
/// <param name="Media">Collection of Attached Artifacts.</param>
public sealed record Message
    (ChatId ChatId, IEnumerable<string> Text, IEnumerable<object>? Artifacts = null) : IMessageContext;