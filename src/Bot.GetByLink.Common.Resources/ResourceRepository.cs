namespace Bot.GetByLink.Common.Resources;

/// <summary>
///     Provides access to text resources.
/// </summary>
public static class ResourceRepository
{
    /// <summary>
    ///     Returns a text resource by key.
    /// </summary>
    /// <param name="key">Resource key.</param>
    /// <returns>Text resource.</returns>
    public static string GetClientResource(string key)
    {
        switch (key)
        {
            case "WrongCommand":
                return ClientResource.WrongCommand;
            case "HelpCommand":
                return ClientResource.HelpCommand;
            default:
                return "-resource empty-";
        }
    }
}