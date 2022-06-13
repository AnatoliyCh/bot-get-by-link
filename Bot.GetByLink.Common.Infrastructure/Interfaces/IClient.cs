using Bot.GetByLink.Common.Infrastructure.Enums;

namespace Bot.GetByLink.Common.Infrastructure.Interfaces;

/// <summary>
///     Client interface.
///     Responsible for interaction with the end platform.
/// </summary>
public interface IClient
{
    /// <summary>
    ///     Gets the current state of the client.
    /// </summary>
    public Status State { get; }

    /// <summary>
    ///     Client start.
    /// </summary>
    /// <returns>Execution result.</returns>
    public Task<bool> Start();

    /// <summary>
    ///     Client stop.
    /// </summary>
    /// <returns>Execution result.</returns>
    public Task<bool> Stop();
}