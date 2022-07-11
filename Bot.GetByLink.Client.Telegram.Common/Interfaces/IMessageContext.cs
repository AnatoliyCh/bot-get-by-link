using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     The interface of an object that contains all the data to send a message.
///     TODO: сменить object на тип.
/// </summary>
public interface IMessageContext : IMessageContext<ChatId, IEnumerable<string>, object>
{
    /// <summary>
    ///     Gets text parsing mode.
    /// </summary>
    public ParseMode? ParseMode { get; init; }
}