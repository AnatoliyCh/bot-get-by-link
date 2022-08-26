using Bot.GetByLink.Common.Enums;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Exceptions;

/// <summary>
/// Exception for the client side of the bot.
/// </summary>
[Serializable]
public sealed class ClientException : GetByLink.Common.Infrastructure.Exceptions.ClientException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientException"/> class.
    /// </summary>
    /// <param name="type">The client-side exception type.</param>
    /// <param name="message">Exception message. Sent to the client.</param>
    /// <param name="chatId">The id of the chat with the client.</param>
    public ClientException(ClientExceptionType type = ClientExceptionType.Allowed, string? message = null, ChatId? chatId = null)
        : base(type, message)
    {
        ChatId = chatId;
    }

    /// <summary>
    /// Gets the id of the chat with the client.
    /// </summary>
    public ChatId? ChatId { get; }

    private ClientException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}