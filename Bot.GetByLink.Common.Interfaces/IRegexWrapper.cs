using System.Text.RegularExpressions;

namespace Bot.GetByLink.Common.Interfaces;

/// <summary>
///     Minimal wrapper for regular expressions.
/// </summary>
public interface IRegexWrapper
{
    /// <summary>
    ///     Gets regular expression.
    /// </summary>
    public string Regex { get; }

    /// <summary>
    ///     Whether a match was found in the input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="options">Parameters for matching.</param>
    /// <returns>True if the regular expression finds a match; otherwise, false.</returns>
    public bool IsMatch(string? input, RegexOptions? options = null);

    /// <summary>
    ///     Finds a substring in an input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="options">Parameters for matching.</param>
    /// <returns>An object that contains information about the match or null.</returns>
    public Match? Match(string? input, RegexOptions? options = null);
}