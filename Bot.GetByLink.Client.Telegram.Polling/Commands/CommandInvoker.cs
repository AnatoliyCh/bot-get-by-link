using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Executes the specified command.
/// </summary>
internal class CommandInvoker : ICommandInvoker<CommandName>
{
    private readonly IDictionary<CommandName, ICommand<CommandName>> commands;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandInvoker" /> class.
    /// </summary>
    /// <param name="client">Telegram Client.</param>
    public CommandInvoker(ITelegramBotClient client)
    {
        if (client == null) throw new ArgumentNullException("client");
        var chatInfoCommand = new ChatInfoCommand(CommandName.ChatInfo, client);
        var sendContentFromUrl = new SendContentFromUrlCommand(CommandName.SendContentFromUrl, client);
        commands = new Dictionary<CommandName, ICommand<CommandName>>
            { { chatInfoCommand.Name, chatInfoCommand }, { sendContentFromUrl.Name, sendContentFromUrl } };
    }

    /// <summary>
    ///     Execute the specified command.
    /// </summary>
    /// <param name="commandName">Command names for telegram client.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public async Task ExecuteCommand(CommandName commandName, object? ctx)
    {
        commands.TryGetValue(commandName, out var command);

        switch (command)
        {
            case IAsyncCommand<CommandName> asyncCommand:
                await asyncCommand.ExecuteAsync(ctx);
                break;
            case ISyncCommand<CommandName> syncCommand:
                syncCommand.Execute(ctx);
                break;
        }
    }
}