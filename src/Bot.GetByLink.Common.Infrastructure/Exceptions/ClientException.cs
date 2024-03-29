﻿using System.Runtime.Serialization;
using Bot.GetByLink.Common.Enums;

namespace Bot.GetByLink.Common.Infrastructure.Exceptions;

/// <summary>
///     Exception for the client side of the bot.
/// </summary>
[Serializable]
public class ClientException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientException" /> class.
    /// </summary>
    /// <param name="type">The client-side exception type.</param>
    /// <param name="message">Exception message. Sent to the client.</param>
    public ClientException(ExceptionType type = ExceptionType.Allowed, string? message = null)
        : base(message)
    {
        Type = type;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientException" /> class.
    /// </summary>
    /// <param name="serializationInfo">
    ///     Stores all the data needed to serialize or deserialize an object. This class cannot be
    ///     inherited.
    /// </param>
    /// <param name="streamingContext">
    ///     Describes the source and destination of a given serialized stream, and provides an
    ///     additional caller-defined context.
    /// </param>
    protected ClientException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }

    /// <summary>
    ///     Gets the client-side exception type.
    /// </summary>
    public ExceptionType Type { get; }
}