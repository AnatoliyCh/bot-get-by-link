namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Executes the specified command.
/// </summary>
/// <typeparam name="TCommandName">Command name type.</typeparam>
public interface ICommandInvoker<in TCommandName>
{
    /// <summary>
    ///     Invokes the specified command.
    /// </summary>
    /// <param name="commandName">Command name.</param>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    Task ExecuteCommand(TCommandName commandName, object? ctx);
}