namespace Blazor.FullCalendar.Models;

public class CalendarEvent : ICalendarEvent
{
    public string? Title { get; init; } = Constants.NoTitle;
    public int DayNumber { get; init; }
}