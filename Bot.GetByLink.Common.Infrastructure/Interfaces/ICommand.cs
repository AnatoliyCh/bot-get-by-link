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
    /// <returns>Empty Task.</returns>
    public Task Execute(object? ctx = null);

    /// <summary>
    ///     Rollback command.
    /// </summary>
    /// <param name="ctx">Context command.</param>
    /// <returns>Empty Task.</returns>
    public Task Undo(object? ctx = null);
}