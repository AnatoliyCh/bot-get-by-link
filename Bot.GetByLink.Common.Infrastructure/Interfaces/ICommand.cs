namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Command Interface.
/// </summary>
public interface ICommand
{
    /// <summary>
    ///     Run command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public void Execute(object? ctx = null);

    /// <summary>
    ///     Rollback command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    public void Undo(object? ctx = null);
}