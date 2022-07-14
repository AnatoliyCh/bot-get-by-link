using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Common.Infrastructure.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
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
internal sealed class ClientPolling : GetByLink.Common.Infrastructure.Abstractions.Client, IDisposable
{
    private readonly ITelegramBotClient client;
    private readonly ICommandInvoker<CommandName> commandInvoker;
    private readonly ILogger logger;
    private readonly string patternCommand = "^\\/[a-zA-Z]+";

    private readonly string patternURL =
        @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";

    private readonly string projectName;

    private readonly ReceiverOptions receiverOptions;

    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientPolling" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    public ClientPolling(
        IBotConfiguration config,
        ILogger<ClientPolling> logger,
        ITelegramBotClient client,
        ICommandInvoker<CommandName> invoker)
    {
        ArgumentNullException.ThrowIfNull(config);

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        commandInvoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        projectName = config.ProjectName ?? throw new NullReferenceException(nameof(projectName));

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
    public override async Task<bool> Start()
    {
        if (State == Status.On || cts is not null) await Stop();
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
    public override Task<bool> Stop()
    {
        Dispose();
        State = Status.Off;
        return Task.FromResult(true);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var text = update.Message!.Text ?? string.Empty;

        // only command message (/**) and URL
        if (!(update.Message!.Type == MessageType.Text &&
              (Regex.IsMatch(text, patternCommand) || Regex.IsMatch(text, patternURL)))) return;

        var words = text.Split(" ");
        if (words is null || words.Length == 0) return;

        // commands
        var firstWord = words.First();
        var commandNameText = Regex.Replace(firstWord, "/", string.Empty);
        if (Regex.IsMatch(firstWord, patternURL)) commandNameText = CommandName.SendContentFromUrl.ToString();

        commandNameText = string.Concat(commandNameText[0].ToString().ToUpper(), commandNameText.AsSpan(1));
        if (!Enum.IsDefined(typeof(CommandName), commandNameText)) return;
        var commandName = Enum.Parse<CommandName>(commandNameText, true);

        await commandInvoker.TryExecuteCommand(commandName, update);
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        await Task.Run(() => logger.LogError(exception, "HandleErrorAsync"));
    }
}