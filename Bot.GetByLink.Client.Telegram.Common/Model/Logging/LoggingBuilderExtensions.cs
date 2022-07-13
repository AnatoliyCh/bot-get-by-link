using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Logging;

/// <summary>
/// Type extension for ILoggingBuilder.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds the given Microsoft.Extensions.Logging.ILoggerProvider to the Microsoft.Extensions.Logging.ILoggingBuilder.
    /// </summary>
    /// <typeparam name="T">The type of the object being added to the DI container.</typeparam>
    /// <param name="builder">The Microsoft.Extensions.Logging.ILoggingBuilder to add the provider to.</param>
    /// <returns>The Microsoft.Extensions.Logging.ILoggingBuilder so that additional calls can be chained.</returns>
    public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder)
        where T : class, ILoggerProvider
    {
        builder.Services.AddSingleton<ILoggerProvider, T>();
        return builder;
    }
}
