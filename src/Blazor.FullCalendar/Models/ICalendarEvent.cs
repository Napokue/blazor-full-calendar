namespace Blazor.FullCalendar.Models;

public interface ICalendarEvent
{
    public string? Title { get; init; }
    public int DayNumber { get; init; }
}