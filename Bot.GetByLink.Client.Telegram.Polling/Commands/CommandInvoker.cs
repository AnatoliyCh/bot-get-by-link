using Bot.GetByLink.Client.Telegram.Polling.Enums;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Executes the specified command.
/// </summary>
internal sealed class CommandInvoker : ICommandInvoker<CommandName>
{
    private readonly IDictionary<CommandName, ICommand<CommandName>> commands;
    private readonly SendMessageCommand sendMessageCommand;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandInvoker" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="proxyServices">Proxy collection.</param>
    public CommandInvoker(IBotConfiguration config, ITelegramBotClient client, IEnumerable<IProxyService> proxyServices)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(proxyServices);

        sendMessageCommand = new SendMessageCommand(client, config.Clients.Telegram.ChatIdLog);
        var chatInfoCommand = new ChatInfoCommand(client, sendMessageCommand);
        var sendContentFromUrl = new SendContentFromUrlCommand(sendMessageCommand, proxyServices);
        commands = new Dictionary<CommandName, ICommand<CommandName>>
        {
            { chatInfoCommand.Name, chatInfoCommand },
            { sendContentFromUrl.Name, sendContentFromUrl },
            { sendMessageCommand.Name, sendMessageCommand }
        };
    }

    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="command">Given command.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public async Task TryExecuteCommand(ICommand<CommandName>? command, object? ctx)
    {
        try
        {
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
        catch (Exception ex)
        {
            await sendMessageCommand.TrySendLogAsync(ex.ToString());
        }
    }

    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="commandName">Command name.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public async Task TryExecuteCommand(CommandName commandName, object? ctx)
    {
        commands.TryGetValue(commandName, out var command);
        await TryExecuteCommand(command, ctx);
    }

    /// <summary>
    ///     Returns a command of the given type.
    /// </summary>
    /// <typeparam name="T">Command type.</typeparam>
    /// <returns>Command of the specified type or null.</returns>
    public T? GetCommand<T>()
        where T : class, ICommand<CommandName>
    {
        foreach (var (_, value) in commands)
            if (value is T command)
                return command;

        return null;
    }
}