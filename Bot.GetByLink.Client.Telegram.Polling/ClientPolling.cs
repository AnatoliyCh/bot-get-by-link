using System.Text.RegularExpressions;
using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Polling;

/// <summary>
///     Telegram client.
///     Connection Type: polling.
/// </summary>
internal class ClientPolling : Common.Infrastructure.Abstractions.Client
{
    private readonly string? chatIdErrorHandling;
    private readonly ITelegramBotClient client;
    private readonly ICommandInvoker<CommandName> commandInvoker;

    private readonly IConfiguration configuration;

    private readonly string patternCommand = "^\\/[a-zA-Z]+";

    private readonly string patternURL =
        @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";

    private readonly ReceiverOptions receiverOptions;

    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientPolling" /> class.
    /// </summary>
    /// <param name="config">Client configuration.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="invoker">Command Executor.</param>
    public ClientPolling(IConfiguration config, ITelegramBotClient client, ICommandInvoker<CommandName> invoker)
    {
        configuration = config ?? throw new ArgumentNullException(nameof(config));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        commandInvoker = invoker ?? throw new ArgumentNullException(nameof(invoker));

        var chatId = configuration.GetValue<string>("Telegram:ChatIdLog");
        if (!string.IsNullOrWhiteSpace(chatId)) chatIdErrorHandling = chatId;
        receiverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.Poll } };
        Console.WriteLine("Initializes a new ClientPolling");
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
            cts = null;
            Console.WriteLine($"{configuration["ProjectName"]}: token is not valid");
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
        if (cts is not null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }

        State = Status.Off;
        return Task.FromResult(true);
    }

    /// <summary>
    ///     Sends a message to the chat for the log.
    /// </summary>
    /// <param name="message">Message content.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task SendTextMessageToLogChatAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message) ||
            string.IsNullOrWhiteSpace(chatIdErrorHandling) ||
            cts is null || State == Status.Off)
            return;

        try
        {
            await client.SendTextMessageAsync(chatIdErrorHandling, message, cancellationToken: cts.Token);
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(client, exception, cts.Token);
        }
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

        await commandInvoker.ExecuteCommand(commandName, update);
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        if (exception is not ApiRequestException apiRequestException) return;
        var exceptionInString = apiRequestException.ToString();
        Console.WriteLine(exceptionInString);

        if (string.IsNullOrWhiteSpace(chatIdErrorHandling) || apiRequestException.Message == "Unauthorized") return;
        await botClient.SendTextMessageAsync(chatIdErrorHandling, exceptionInString, cancellationToken: ct);
    }
}