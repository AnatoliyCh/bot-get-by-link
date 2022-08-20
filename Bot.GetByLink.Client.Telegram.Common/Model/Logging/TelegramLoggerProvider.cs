using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Microsoft.Extensions.Logging;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Logging;

/// <summary>
///     Telegram logging provider.
/// </summary>
/// <typeparam name="TCommandName">Command name type.</typeparam>
public sealed class TelegramLoggerProvider<TCommandName> : ILoggerProvider
{
    private IBotConfiguration? botConfiguration;
    private Func<object?, Task>? sendMessage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelegramLoggerProvider{TCommandName}" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="sendMessageCommand">Command to send messages to telegram.</param>
    public TelegramLoggerProvider(IBotConfiguration config, ICommand<TCommandName> sendMessageCommand)
    {
        botConfiguration = config ?? throw new ArgumentNullException(nameof(config));
        ArgumentNullException.ThrowIfNull(sendMessageCommand);
        sendMessage = ((IAsyncCommand<TCommandName>)sendMessageCommand).ExecuteAsync;
    }

    /// <summary>
    ///     Creates a new Microsoft.Extensions.Logging.ILogger instance.
    ///     ! Description taken from ILoggerProvider.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The instance of Microsoft.Extensions.Logging.ILogger that was created.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new TelegramLogger(sendMessage, botConfiguration?.Clients.Telegram.ChatIdLog);
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        botConfiguration = null;
        sendMessage = null;
    }
}