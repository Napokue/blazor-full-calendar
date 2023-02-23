namespace Blazor.FullCalendar.Models;

public interface ICalendarEvent : ICloneable
{
    public string? Title { get; set; }
    public int DayNumber { get; init; }
}