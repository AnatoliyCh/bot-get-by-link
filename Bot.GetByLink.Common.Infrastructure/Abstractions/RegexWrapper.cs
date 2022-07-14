using System.Text.RegularExpressions;
using Bot.GetByLink.Common.Infrastructure.Interfaces;

namespace Bot.GetByLink.Common.Infrastructure.Abstractions;

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

    /// <inheritdoc />
    public string Regex { get; }

    /// <inheritdoc />
    public virtual bool IsMatch(string? input, RegexOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return System.Text.RegularExpressions.Regex.IsMatch(input, Regex, options ?? RegexOptions.None);
    }

    /// <inheritdoc />
    public virtual Match? Match(string? input, RegexOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        return System.Text.RegularExpressions.Regex.Match(input, Regex, options ?? RegexOptions.None);
    }
}