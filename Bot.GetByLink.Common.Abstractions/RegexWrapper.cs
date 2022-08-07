using System.Text.RegularExpressions;

namespace Bot.GetByLink.Common.Abstractions;

/// <summary>
///     Minimal wrapper for regular expressions.
///     Description of basic methods.
/// </summary>
public abstract class RegexWrapper : IRegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegexWrapper" /> class.
    /// </summary>
    /// <param name="regex">Regular expressions.</param>
    protected RegexWrapper(string regex)
    {
        Regex = regex ?? throw new ArgumentNullException(nameof(regex));
    }

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
    public virtual bool IsMatch(string? input, RegexOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return System.Text.RegularExpressions.Regex.IsMatch(input, Regex, options ?? RegexOptions.None);
    }

    /// <summary>
    ///     Finds a substring in an input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="options">Parameters for matching.</param>
    /// <returns>An object that contains information about the match or null.</returns>
    public virtual Match? Match(string? input, RegexOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        return System.Text.RegularExpressions.Regex.Match(input, Regex, options ?? RegexOptions.None);
    }
}