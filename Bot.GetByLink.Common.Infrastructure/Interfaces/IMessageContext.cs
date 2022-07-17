namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     The interface of an object that contains all the data to send a message.
/// </summary>
/// <typeparam name="TChatId">Chat ID type.</typeparam>
/// <typeparam name="TText">Text type.</typeparam>
/// <typeparam name="TArtifact">Type of attached artifacts.</typeparam>
public interface IMessageContext<TChatId, TText, TArtifact>
{
    /// <summary>
    ///     Gets chat ID to send the message.
    /// </summary>
    TChatId ChatId { get; init; }

    /// <summary>
    ///     Gets message text.
    /// </summary>
    TText Text { get; init; }

    /// <summary>
    ///     Gets collection of Attached Artifacts.
    /// </summary>
    IEnumerable<TArtifact>? Artifacts { get; init; }
}