namespace Elyssa.PublicApi.Shared.Helpers;

public static class StringHelper
{
    public static string ToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }
}
