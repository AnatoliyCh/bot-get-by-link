namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Executes the specified command.
/// </summary>
/// <typeparam name="TCommandName">Command name type.</typeparam>
public interface ICommandInvoker<in TCommandName>
{
    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="command">Given command.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public Task TryExecuteCommandAsync(ICommand<TCommandName>? command, object? ctx);

    /// <summary>
    ///     Calls the given command.
    /// </summary>
    /// <param name="commandName">Command name.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public Task TryExecuteCommandAsync(TCommandName commandName, object? ctx);

    /// <summary>
    ///     Returns a command of the given type.
    /// </summary>
    /// <typeparam name="T">Command type.</typeparam>
    /// <returns>Command of the specified type or null.</returns>
    public T? GetCommand<T>()
        where T : class, ICommand<TCommandName>;
}