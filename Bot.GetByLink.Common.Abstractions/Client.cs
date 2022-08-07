namespace Bot.GetByLink.Common.Abstractions;

/// <summary>
///     Base abstract client class.
///     Responsible for interaction with the end platform.
/// </summary>
public abstract class Client : IClient
{
    /// <summary>
    ///     Gets or sets the current state of the client.
    /// </summary>
    public Status State { get; protected set; } = Status.Off;

    /// <summary>
    ///     Client start.
    /// </summary>
    /// <returns>Execution result.</returns>
    public abstract Task<bool> StartAsync();

    /// <summary>
    ///     Client stop.
    /// </summary>
    /// <returns>Execution result.</returns>
    public abstract Task<bool> StopAsync();
}
