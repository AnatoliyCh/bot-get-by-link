using Bot.GetByLink.Common.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     The interface of an object that contains all the data to send a message.
/// </summary>
public interface IMessageContext : IMessageContext<ChatId, IEnumerable<string>, IAlbumInputMedia>
{
    /// <summary>
    ///     Gets text parsing mode.
    /// </summary>
    public ParseMode? ParseMode { get; init; }
}