using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Logging;

/// <summary>
///     Object for logging in telegram.
/// </summary>
internal sealed class TelegramLogger : ILogger
{
    private ChatId? logChatId;
    private Func<object?, Task>? sendMessage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelegramLogger" /> class.
    /// </summary>
    /// <param name="sendMessage">Function of sending a message to telegram.</param>
    /// <param name="logChatId">Chat for logging issues (database not used).</param>
    public TelegramLogger(Func<object?, Task>? sendMessage, string? logChatId = null)
    {
        this.sendMessage = sendMessage ?? throw new ArgumentNullException(nameof(sendMessage));
        this.logChatId = !string.IsNullOrWhiteSpace(logChatId) ? new ChatId(logChatId) : null;
    }

    /// <summary>
    ///     Begins a logical operation scope.
    ///     ! Description taken from ILogger.
    /// </summary>
    /// <typeparam name="TState">The identifier for the scope.</typeparam>
    /// <param name="state">The type of the state to begin scope for.</param>
    /// <returns> An System.IDisposable that ends the logical operation scope on dispose.</returns>
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        sendMessage = null;
        logChatId = null;
        return default!;
    }

    /// <summary>
    ///     Indicates if the logger is available for use.
    /// </summary>
    /// <param name="logLevel">Logging level.</param>
    /// <returns>true if enabled.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    /// <summary>
    ///     Writes a log entry.
    ///     ! Description taken from ILogger.
    /// </summary>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">Id of the event.</param>
    /// <param name="state">The entry to be written. Can be also an object.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="formatter"> Function to create a System.String message of the state and exception.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (formatter is null || sendMessage is null || logChatId is null) return;
        var message = new Message(logChatId, new List<string> { $"{formatter(state, exception)}\n{exception}" });
        sendMessage?.Invoke(message);
    }
}