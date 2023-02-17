namespace Blazor.FullCalendar.Services;

internal static class WeekService
{
    private const DayOfWeek DefaultDayOfWeek = DayOfWeek.Sunday;
    private static readonly IReadOnlyList<string> Days;

    static WeekService()
    {
        Days = new List<string>
        {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"
        };
    }

    internal static IReadOnlyList<string> GetDaysBasedOnStartingDay(DayOfWeek startingDay)
    {
        if (startingDay == DefaultDayOfWeek)
        {
            return Days;
        }
        
        return Days;
    } 
}