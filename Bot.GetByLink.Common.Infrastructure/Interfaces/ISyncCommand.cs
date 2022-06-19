namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Synchronous command interface.
/// </summary>
/// <typeparam name="TName">Command name type.</typeparam>
public interface ISyncCommand<out TName> : ICommand<TName>
{
    /// <summary>
    ///     Execute a synchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public void Execute(object? ctx);

    /// <summary>
    ///     Rollback synchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public void Undo(object? ctx);
}