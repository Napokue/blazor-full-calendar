using Blazor.FullCalendar.Models;

namespace Blazor.FullCalendar.Services;

public class CalendarEventFactory : ICalendarEventFactory
{
    public ICalendarEvent Create(DateOnly day)
    {
        return new CalendarEvent
        {
            DayNumber = day.DayNumber,
            Title = $"Test {day.DayNumber}"
        };
    }
}