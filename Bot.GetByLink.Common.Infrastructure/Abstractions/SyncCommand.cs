using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

/// <summary>
///     The base class for an synchronous command.
/// </summary>
/// <typeparam name="TName">Command name type.</typeparam>
public abstract class SyncCommand<TName> : ISyncCommand<TName>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SyncCommand{TName}" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    protected SyncCommand(TName name)
    {
        Name = name;
    }

    /// <summary>
    ///     Gets command name.
    /// </summary>
    public TName Name { get; }

    /// <summary>
    ///     Execute a synchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public abstract void Execute(object? ctx);

    /// <summary>
    ///     Rollback synchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <exception cref="NotImplementedException">Method is not implemented.</exception>
    public void Undo(object? ctx)
    {
        throw new NotImplementedException();
    }
}