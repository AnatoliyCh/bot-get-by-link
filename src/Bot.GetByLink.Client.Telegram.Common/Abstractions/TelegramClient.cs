using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Bot.GetByLink.Common.Resources;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Bot.GetByLink.Client.Telegram.Common.Model.Message;

namespace Bot.GetByLink.Client.Telegram.Common.Abstractions;

/// <summary>
///     Telegram client.
/// </summary>
public abstract class TelegramClient : GetByLink.Common.Abstractions.Client, IDisposable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TelegramClient" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <exception cref="ArgumentNullException">Any argument is null.</exception>
    /// <exception cref="NullReferenceException">The regexWrappers collection is empty.</exception>
    protected TelegramClient(
        ILogger logger,
        IBotConfiguration config,
        ITelegramBotClient client,
        ICommandInvoker<CommandName> invoker,
        IEnumerable<IRegexWrapper> regexWrappers)
    {
        ArgumentNullException.ThrowIfNull(config);

        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Client = client ?? throw new ArgumentNullException(nameof(client));
        CommandInvoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        ProjectName = config.ProjectName ?? throw new NullReferenceException(nameof(ProjectName));
        if (regexWrappers is null || !regexWrappers.Any()) throw new ArgumentNullException(nameof(regexWrappers));

        RegexWrappers = regexWrappers;

        ReceiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message },
            ThrowPendingUpdates = config.Clients.Telegram.ThrowPendingUpdates
        };
    }

    /// <summary>
    ///     Gets logging service.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    ///     Gets the Telegram client.
    /// </summary>
    protected ITelegramBotClient Client { get; }

    /// <summary>
    ///     Gets the Command Executor.
    /// </summary>
    protected ICommandInvoker<CommandName> CommandInvoker { get; }

    /// <summary>
    ///     Gets the Project Name.
    /// </summary>
    protected string ProjectName { get; }

    /// <summary>
    ///     Gets the Receiver Options.
    /// </summary>
    protected ReceiverOptions ReceiverOptions { get; }

    /// <summary>
    ///     Gets the Regex Wrappers.
    /// </summary>
    protected IEnumerable<IRegexWrapper> RegexWrappers { get; }

    /// <summary>
    ///     Token Cancellation.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Processing messages from the client.
    /// </summary>
    /// <param name="update">This object represents an incoming update.</param>
    /// <returns>Empty Task.</returns>
    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message is null ||
            update.Message.Chat is null ||
            update.Message.Text is null ||
            update.Message.Type != MessageType.Text) return;

        var text = update.Message.Text;
        var context =
            RegexWrappers
                .FirstOrDefault(wrapper => wrapper.IsMatch(text, RegexOptions.IgnoreCase | RegexOptions.Multiline))?
                .Match(text, RegexOptions.IgnoreCase | RegexOptions.Multiline)?.Value;

        // only command message (/**) and URL
        if (string.IsNullOrWhiteSpace(context)) return;

        // commands
        var commandName = GetCommandNameByString(context);
        if (commandName is null)
        {
            await CommandInvoker.TryExecuteCommandAsync(
                CommandName.SendMessage,
                new Message(update.Message.Chat.Id,
                    new[] { ResourceRepository.GetClientResource(ClientResource.WrongCommand) }));
            return;
        }

        var isSuccessfully = await CommandInvoker.TryExecuteCommandAsync((CommandName)commandName, update);
        if (isSuccessfully) await Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
    }

    /// <summary>
    ///     Client stop.
    /// </summary>
    /// <returns>Execution result.</returns>
    public override Task<bool> StopAsync()
    {
        Dispose();
        State = Status.Off;
        return Task.FromResult(true);
    }

    /// <summary>
    ///     Returns the text name of the command in the enum key.
    /// </summary>
    /// <param name="input">Text command.</param>
    /// <returns>Enum key or null.</returns>
    protected CommandName? GetCommandNameByString(string input)
    {
        foreach (var regex in RegexWrappers)
            switch (regex)
            {
                case UrlRegexWrapper urlRegex:
                    if (urlRegex.IsMatch(input)) return CommandName.SendContentFromUrl;
                    break;
                case CommandRegexWrapper commandRegex:
                    if (!commandRegex.IsMatch(input)) return null;
                    var commandNameText = Regex.Replace(input, "/", string.Empty);
                    commandNameText = string.Concat(commandNameText[0].ToString().ToUpper(), commandNameText.AsSpan(1));
                    Enum.TryParse(typeof(CommandName), commandNameText, true, out var result);
                    return (CommandName?)result;
            }

        return null;
    }

    /// <summary>
    ///     Releases all resources used by the current instance TelegramClient.
    /// </summary>
    /// <param name="disposing">Whether to free resources.</param>
    protected virtual void Dispose(bool disposing)
    {
    }
}