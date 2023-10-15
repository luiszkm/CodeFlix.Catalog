
namespace CodeFlix.Catalog.E2ETests.Extensions;
internal static class DateTimeExtensions
{
    public static DateTime TrimMilliseconds(
        this DateTime dateTime)
    {
        return new DateTime(
                       dateTime.Year,
                       dateTime.Month,
                       dateTime.Day,
                       dateTime.Hour,
                       dateTime.Minute,
                       dateTime.Second,
                       0,
                       dateTime.Kind);
    }
}
