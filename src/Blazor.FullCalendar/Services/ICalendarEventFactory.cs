using Blazor.FullCalendar.Models;

namespace Blazor.FullCalendar.Services;

public interface ICalendarEventFactory
{
    ICalendarEvent Create(DateOnly day);
}