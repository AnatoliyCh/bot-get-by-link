using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     The interface of an object that contains all the data to send a message.
///     TODO: сменить object на тип.
/// </summary>
public interface IMessageContext : IMessageContext<ChatId, IEnumerable<string>, object>
{
}