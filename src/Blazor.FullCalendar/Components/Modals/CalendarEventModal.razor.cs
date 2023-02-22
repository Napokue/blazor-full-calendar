using System.Reflection;
using Blazor.FullCalendar.Attributes;
using Blazor.FullCalendar.Models;
using Blazor.FullCalendar.Utils;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.FullCalendar.Components.Modals;

public partial class CalendarEventModal<TCalendarEvent> : ComponentBase where TCalendarEvent : ICalendarEvent
{
    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

    [Parameter]
    public TCalendarEvent CalendarEvent { get; set; } = default!;

    private PropertyInfo[] _calendarEventProperties = default!;

    async Task Create() => await BlazoredModal.CloseAsync(ModalResult.Ok(true));
    async Task Cancel() => await BlazoredModal.CancelAsync();

    protected override Task OnInitializedAsync()
    {
        _calendarEventProperties = CalendarEvent.GetType().ContainsAttribute<CalendarEventFieldAttribute>() 
            ? CalendarEvent.GetType().GetProperties() 
            : CalendarEvent.GetType().GetProperties().Where(ReflectionUtils.ContainsAttribute<CalendarEventFieldAttribute>).ToArray();

        return Task.CompletedTask;
    }
}
