using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

/// <summary>
///     Base abstract command class.
/// </summary>
public abstract class Command : ICommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Command" /> class.
    /// </summary>
    /// <param name="name">Command name.</param>
    protected Command(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    ///     Gets command name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Run command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public abstract void Execute(object? ctx = null);

    /// <summary>
    ///     Rollback command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public abstract void Undo(object? ctx = null);
}