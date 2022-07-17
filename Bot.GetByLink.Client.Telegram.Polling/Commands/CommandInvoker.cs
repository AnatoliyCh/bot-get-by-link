using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Model;
using Bot.GetByLink.Common.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Bot.GetByLink.Client.Telegram.Polling.Commands;

/// <summary>
///     Executes the specified command.
/// </summary>
internal sealed class CommandInvoker : ICommandInvoker<CommandName>
{
    private readonly IDictionary<CommandName, ICommand<CommandName>> commands;
    private readonly ILogger logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandInvoker" /> class.
    /// </summary>
    /// <param name="config">Bot configuration.</param>
    /// <param name="logger">Interface for logging.</param>
    /// <param name="client">Telegram Client.</param>
    /// <param name="proxyServices">Proxy collection.</param>
    /// <param name="serviceCommands">Service commands.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    public CommandInvoker(
        IBotConfiguration config,
        ILogger<CommandInvoker> logger,
        ITelegramBotClient client,
        IEnumerable<IProxyService> proxyServices,
        IEnumerable<ICommand<CommandName>> serviceCommands,
        IEnumerable<IRegexWrapper> regexWrappers)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(proxyServices);

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        var sendMessageCommand =
            (SendMessageCommand)serviceCommands.First(command => command.Name == CommandName.SendMessage) ??
            throw new NullReferenceException("Command: SendMessage is null");
        var chatInfoCommand = new ChatInfoCommand(client, sendMessageCommand);
        var formaterContent = new ProxyResponseFormatter();
        var sendContentFromUrl =
            new SendContentFromUrlCommand(sendMessageCommand, proxyServices, regexWrappers, formaterContent);

        commands = serviceCommands
            .Concat(new List<ICommand<CommandName>> { chatInfoCommand, sendContentFromUrl })
            .DistinctBy(command => command.Name)
            .ToDictionary(command => command.Name, command => command);
    }

    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="command">Given command.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public async Task TryExecuteCommandAsync(ICommand<CommandName>? command, object? ctx)
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
            var message = $"ICommand: {command?.Name.ToString()}";
            logger.LogError(ex, message);
        }
    }

    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="commandName">Command name.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public async Task TryExecuteCommandAsync(CommandName commandName, object? ctx)
    {
        commands.TryGetValue(commandName, out var command);
        await TryExecuteCommandAsync(command, ctx);
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