using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Regexs;

/// <summary>
///     Regular expression for Command.
///     RU and ENG locale.
/// </summary>
/// xxxx/command{1,20}xxxx
public sealed class CommandRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandRegexWrapper" /> class.
    /// </summary>
    public CommandRegexWrapper()
        : base(@"\/[0-9a-zA-Zа-яА-я]{1,20}")
    {
    }

    /// <summary>
    ///     Finds a substring in an input string.
    ///     If settings are not specified, it ignores the case and multiline.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="options">Parameters for matching.</param>
    /// <returns>An object that contains information about the match or null.</returns>
    public override Match? Match(string? input, RegexOptions? options = null)
    {
        return base.Match(input, options ?? RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }
}