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

        var startingDayInt = (int) startingDay;

        var sortedDays = new List<string>();
        sortedDays.InsertRange(0, Days.Skip(startingDayInt));

        for (var i = 0; i < startingDayInt; i++)
        {
            sortedDays.Add(Days[i]);
        }
        
        return sortedDays;
    } 
}