namespace Elyssa.PublicApi.Shared.Extensions;

public static class DateTimeExtensions
{
    public static string ToFriendlyString(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
