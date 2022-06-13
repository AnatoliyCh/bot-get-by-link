using System.Text.RegularExpressions;
using Bot.GetByLink.Proxy.Reddit;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.GetByLink.Client.Telegram.Polling;

/// <summary>
///     Client state types.
/// </summary>
public enum Status
{
    /// <summary>
    ///     Represents a running client.
    /// </summary>
    On,

    /// <summary>
    ///     Represents a stopped client.
    /// </summary>
    Off
}

/// <summary>
///     Telegram client.
///     Connection Type: polling.
/// </summary>
internal class ClientPolling
{
    private readonly string? chatIdErrorHandling;
    private readonly ITelegramBotClient client;
    private readonly IConfiguration configuration;
    private readonly string patternCommand = "^\\/[a-zA-Z]+";
    private readonly ReceiverOptions receiverOptions;

    private CancellationTokenSource? cts;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientPolling" /> class.
    /// </summary>
    /// <param name="configuration">Client configuration.</param>
    public ClientPolling(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        var tokenClientTelegram = configuration.GetValue<string>("telegram:token-client");
        if (!string.IsNullOrWhiteSpace(tokenClientTelegram)) client = new TelegramBotClient(tokenClientTelegram);
        else throw new ArgumentException("telegram:token-client");

        var chatId = configuration.GetValue<string>("telegram:chat-id-log");
        if (!string.IsNullOrWhiteSpace(chatId)) chatIdErrorHandling = chatId;

        receiverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.Poll } };
    }

    /// <summary>
    ///     Gets the current state of the client.
    /// </summary>
    public Status Status { get; private set; } = Status.Off;

    /// <summary>
    ///     Client launch or reset.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    public async Task<bool> Start()
    {
        if (Status == Status.On || cts is not null) Stop();
        cts = new CancellationTokenSource();
        var validToken = await client.TestApiAsync(cts.Token);
        if (!validToken)
        {
            cts = null;
            Console.WriteLine($"{configuration["project-name"]}: token is not valid");
            return false;
        }

        client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);
        Status = Status.On;
        return true;
    }

    /// <summary>
    ///     Client stop.
    /// </summary>
    public void Stop()
    {
        if (cts is null) return;
        Status = Status.Off;
        cts.Cancel();
        cts.Dispose();
    }

    /// <summary>
    ///     Sends a message to the chat for the log.
    /// </summary>
    /// <param name="message">Message content.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task SendTextMessageToLogChatAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(chatIdErrorHandling) || cts is null ||
            Status == Status.Off) return;
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
        // only text messages (/***)
        //if (!(update.Message!.Type == MessageType.Text &&
        //      Regex.IsMatch(update.Message!.Text ?? string.Empty, patternCommand))) return;

        var chatId = update.Message.Chat.Id;
        var words = update.Message.Text?.Split(" ");
        if (words is null || words.Length == 0) return;

        if (words[0] == "/chatInfo")
        {
            var chatInfo = await client.GetChatAsync(chatId, ct);
            var from = update.Message.From;
            var response =
                $"Chat Id: {chatInfo.Id} \n" +
                $"Chat Type: {chatInfo.Type} \n" +
                $"From User: {from?.FirstName} {from?.LastName} @{from?.Username}";
            await botClient.SendTextMessageAsync(chatId, response, cancellationToken: ct);
        }

        if (update.Message.Text != null && Regex.IsMatch(update.Message.Text, @"https?://www.reddit.com/r/\S+/comments/\S+"))
        {
            var proxyReddit = new ProxyReddit("", "");
            var linkPost = Regex.Match(update.Message.Text, @"https?://www.reddit.com/r/\S+/comments/\S+").Value;
            var cutUrlPost = linkPost.Substring(linkPost.IndexOf("comments/") + 9);
            var idPost = cutUrlPost.Substring(0, cutUrlPost.IndexOf("/"));
            var contentPost = proxyReddit.GetPostId(idPost);
            var response = string.Empty;
            if (!string.IsNullOrWhiteSpace(contentPost.UrlPicture)) response += contentPost.UrlPicture + " ";
            if (!string.IsNullOrWhiteSpace(contentPost.UrlVideo)) response += contentPost.UrlVideo + " ";
            response += contentPost.Text;
            response = response.Replace(".", @"\.").Replace("_", @"\_").Replace("#", @"\#").Replace("=", @"\=").Replace("!", @"\!");
            if (response.Length > 4096) response = response.Substring(0, 4096);
            await botClient.SendTextMessageAsync(chatId, response, ParseMode.MarkdownV2, cancellationToken: ct);
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        if (exception is not ApiRequestException apiRequestException) return;
        var exceptionInString = apiRequestException.ToString();
        if (string.IsNullOrWhiteSpace(chatIdErrorHandling) || apiRequestException.Message == "Unauthorized")
        {
            Console.WriteLine(exceptionInString);
            return;
        }

        await botClient.SendTextMessageAsync(chatIdErrorHandling, exceptionInString, cancellationToken: ct);
    }
}