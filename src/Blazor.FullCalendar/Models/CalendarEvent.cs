using Blazor.FullCalendar.Attributes;

namespace Blazor.FullCalendar.Models;

public class CalendarEvent : ICalendarEvent
{
    [CalendarEventField]
    public string? Title { get; init; } = Constants.NoTitle;
    public int DayNumber { get; init; }
}