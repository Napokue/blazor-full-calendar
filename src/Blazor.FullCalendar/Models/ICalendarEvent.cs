namespace Blazor.FullCalendar.Models;

public interface ICalendarEvent
{
    public string? Title { get; set; }
    public int DayNumber { get; init; }
}