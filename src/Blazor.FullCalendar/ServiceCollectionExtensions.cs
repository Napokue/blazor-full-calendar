using Blazor.FullCalendar.Services;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.FullCalendar;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFullCalendar(this IServiceCollection services)
    {
        services.AddScoped<ICalendarEventFactory, CalendarEventFactory>();
        
        if (services.All(service => service.ServiceType != typeof(IModalService)))
        {
            services.AddBlazoredModal();
        }

        return services;
    }
}