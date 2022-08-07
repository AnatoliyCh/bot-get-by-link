using Bot.GetByLink.Common.Abstractions;
using System.Text.RegularExpressions;

namespace Bot.GetByLink.Client.Telegram.Common.Model;

/// <summary>
///     Regular expression for Command.
/// </summary>
public sealed class CommandRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandRegexWrapper" /> class.
    /// </summary>
    public CommandRegexWrapper()
        : base(@"^\/[a-zA-Z]+")
    {
    }

    /// <summary>
    ///     Finds a substring in an input string.
    ///     If settings are not specified, it ignores the case.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="options">Parameters for matching.</param>
    /// <returns>An object that contains information about the match or null.</returns>
    public override Match? Match(string? input, RegexOptions? options = null)
    {
        return base.Match(input, options ?? RegexOptions.IgnoreCase);
    }
}