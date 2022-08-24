using Bot.GetByLink.Common.Abstractions;

namespace Bot.GetByLink.Proxy.Vk.Model.Regexs;

/// <summary>
///     Regular expression for VK resource Id.
/// </summary>
/// -000000000_0
/// -000000000_00
/// 000000000_000
internal sealed class IdResourceRegexWrapper : RegexWrapper
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IdResourceRegexWrapper" /> class.
    /// </summary>
    public IdResourceRegexWrapper()
        : base(@"-?\d+_\d+$")
    {
    }
}