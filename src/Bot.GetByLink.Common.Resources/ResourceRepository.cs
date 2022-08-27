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
    public static string GetClientResource(Enums.ClientResource key)
    {
        switch (key)
        {
            case Enums.ClientResource.WrongCommand:
                return ClientResource.WrongCommand;
            case Enums.ClientResource.HelpCommand:
                return ClientResource.HelpCommand;
            case Enums.ClientResource.FailedGetResource:
                return ClientResource.FailedGetResource;
            default:
                return "-resource empty-";
        }
    }
}