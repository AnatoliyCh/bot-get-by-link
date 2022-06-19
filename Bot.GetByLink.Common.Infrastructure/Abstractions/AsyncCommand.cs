using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

/// <summary>
///     The base class for an asynchronous command.
/// </summary>
/// <typeparam name="TName">Command name type.</typeparam>
public abstract class AsyncCommand<TName> : IAsyncCommand<TName>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncCommand{TName}" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    protected AsyncCommand(TName name)
    {
        Name = name;
    }

    /// <summary>
    ///     Gets command name.
    /// </summary>
    public TName Name { get; }

    /// <summary>
    ///     Execute an asynchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public abstract Task ExecuteAsync(object? ctx);

    /// <summary>
    ///     Rollback an asynchronous command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented.</exception>
    public virtual Task UndoAsync(object? ctx)
    {
        throw new NotImplementedException();
    }
}