namespace Bot.GetByLink.Common.Infrastructure.Model;

/// <summary>
///     Class for extention IEnumerable.
/// </summary>
public static class EnumerableExtention
{
    /// <summary>
    ///     Splits an IEnumerable into several smaller IEnumerables.
    /// </summary>
    /// <typeparam name="T">The type of the IEnumerable.</typeparam>
    /// <param name="enumerable">The IEnumerable to split.</param>
    /// <param name="size">The size of the smaller IEnumerables.</param>
    /// <returns>An IEnumerable containing smaller IEnumerables.</returns>
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int size)
    {
        for (var i = 0; i < (float)enumerable.Count() / size; i++) yield return enumerable.Skip(i * size).Take(size);
    }
}