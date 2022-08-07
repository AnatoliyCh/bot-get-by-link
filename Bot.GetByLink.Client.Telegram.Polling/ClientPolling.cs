using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Polling;

/// <summary>
///     Telegram client.
///     Connection Type: polling.
/// </summary>
internal sealed class ClientPolling : GetByLink.Common.Abstractions.Client, IDisposable
{
    private readonly ITelegramBotClient client;
    private readonly ICommandInvoker<CommandName> commandInvoker;
    private readonly ILogger logger;

    private readonly string projectName;

    private readonly ReceiverOptions receiverOptions;
    private readonly IEnumerable<IRegexWrapper> regexWrappers;

    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientPolling" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    public ClientPolling(
        IBotConfiguration config,
        ILogger<ClientPolling> logger,
        ITelegramBotClient client,
        ICommandInvoker<CommandName> invoker,
        IEnumerable<IRegexWrapper> regexWrappers)
    {
        ArgumentNullException.ThrowIfNull(config);

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        commandInvoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        projectName = config.ProjectName ?? throw new NullReferenceException(nameof(projectName));
        if (regexWrappers is null || !regexWrappers.Any()) throw new ArgumentNullException(nameof(regexWrappers));

        this.regexWrappers = regexWrappers;

        receiverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.Poll } };
    }

    /// <summary>
    ///     /// Token Cancellation.
    /// </summary>
    public void Dispose()
    {
        if (cts is null) return;
        cts.Cancel();
        cts.Dispose();
        cts = null;
    }

    /// <summary>
    ///     Client launch or reset.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    public override async Task<bool> StartAsync()
    {
        if (State == Status.On || cts is not null) await StopAsync();
        cts = new CancellationTokenSource();
        var validToken = await client.TestApiAsync(cts.Token);
        if (!validToken)
        {
            Dispose();
            logger.LogCritical($"{projectName}: token is not valid");
            return false;
        }

        client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);
        State = Status.On;
        return true;
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

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var text = update.Message!.Text ?? string.Empty;

        // only command message (/**) and URL
        if (update.Message!.Type != MessageType.Text && regexWrappers.Any(regex => regex.IsMatch(text))) return;

        var words = text.Split(" ");
        if (words is null || words.Length == 0) return;

        // commands
        var firstWord = words.First();
        var commandName = GetCommandNameByString(firstWord);
        if (commandName is null) return;

        await commandInvoker.TryExecuteCommandAsync((CommandName)commandName, update);
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        await Task.Run(() => logger.LogError(exception, "HandleErrorAsync"));
    }

    private CommandName? GetCommandNameByString(string input)
    {
        foreach (var regex in regexWrappers)
            switch (regex)
            {
                case UrlRegexWrapper urlRegex:
                    if (urlRegex.IsMatch(input)) return CommandName.SendContentFromUrl;
                    break;
                case CommandRegexWrapper commandRegex:
                    if (!commandRegex.IsMatch(input)) return null;
                    var commandNameText = Regex.Replace(input, "/", string.Empty);
                    commandNameText = string.Concat(commandNameText[0].ToString().ToUpper(), commandNameText.AsSpan(1));
                    if (!Enum.IsDefined(typeof(CommandName), commandNameText)) return null;
                    return Enum.Parse<CommandName>(commandNameText, true);
            }

        return null;
    }
}