using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Common.Interfaces;

/// <summary>
///     The interface of an object that build messages for telegram.
/// </summary>
public interface IBuilderMessage : IBuilderMessage<IProxyContent, IBuilderMessage, IMessageContext>
{
    /// <summary>
    ///     Set header in builder.
    /// </summary>
    /// <returns>This builder.</returns>
    public IBuilderMessage SetHeaders();

    /// <summary>
    ///     Add chat id in builder.
    /// </summary>
    /// <param name="chatId">Chat id.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage AddChatId(long chatId);

    /// <summary>
    ///     Set parse mode id in builder.
    /// </summary>
    /// <param name="parseMode">Parse mode.</param>
    /// <returns>This builder.</returns>
    public IBuilderMessage SetParseMode(ParseMode parseMode);
}