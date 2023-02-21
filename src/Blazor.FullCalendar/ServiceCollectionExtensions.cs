using Blazor.FullCalendar.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FullCalendar;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFullCalendar(this IServiceCollection services)
    {
        return services.AddScoped<ICalendarEventFactory, CalendarEventFactory>();
    }
}