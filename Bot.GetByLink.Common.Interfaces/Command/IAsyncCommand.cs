namespace Bot.GetByLink.Common.Interfaces.Command;

/// <summary>
///     Asynchronous command interface.
/// </summary>
/// <typeparam name="TName">Command name type.</typeparam>
public interface IAsyncCommand<out TName> : ICommand<TName>
{
    /// <summary>
    ///     Execute an asynchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public Task ExecuteAsync(object? ctx);

    /// <summary>
    ///     Rollback an asynchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public Task UndoAsync(object? ctx);
}
