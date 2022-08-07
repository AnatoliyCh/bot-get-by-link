namespace Bot.GetByLink.Common.Interfaces;

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
    public Task<bool> StartAsync();

    /// <summary>
    ///     Client stop.
    /// </summary>
    /// <returns>Execution result.</returns>
    public Task<bool> StopAsync();
}
