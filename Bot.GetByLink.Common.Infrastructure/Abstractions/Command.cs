using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

/// <summary>
///     Base abstract command class.
/// </summary>
/// <typeparam name="TName">Command name type (enums).</typeparam>
public abstract class Command<TName> : ICommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Command{TName}" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    protected Command(TName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    ///     Gets command name.
    /// </summary>
    public TName Name { get; }

    /// <summary>
    ///     Run command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public abstract Task Execute(object? ctx = null);

    /// <summary>
    ///     Rollback command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public virtual Task Undo(object? ctx = null)
    {
        throw new NotImplementedException();
    }
}