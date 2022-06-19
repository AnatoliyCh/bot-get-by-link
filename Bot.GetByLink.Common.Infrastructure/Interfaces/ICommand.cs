namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Basic Command Interface.
/// </summary>
/// <typeparam name="TName">Command name type.</typeparam>
public interface ICommand<out TName>
{
    /// <summary>
    ///     Gets command name.
    /// </summary>
    public TName Name { get; }
}