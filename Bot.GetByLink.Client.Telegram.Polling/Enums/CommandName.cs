namespace Bot.GetByLink.Client.Telegram.Polling.Enums;

/// <summary>
///     Command names for telegram client.
/// </summary>
public enum CommandName
{
    /// <summary>
    ///     Information about the current chat.
    /// </summary>
    ChatInfo,

    /// <summary>
    ///     Sends a message to the client.
    /// </summary>
    SendMessage,

    /// <summary>
    ///     Send content post from url in client.
    /// </summary>
    SendContentFromUrl
}