using System.Linq.Expressions;
using System.Reflection;
using Blazor.FullCalendar.Attributes;
using Blazor.FullCalendar.Models;
using Blazor.FullCalendar.Utils;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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
    
    private readonly RenderFragment<CalendarEventProperty> _createInputField
        = calendarEventProperty => builder =>
        {
            var property = calendarEventProperty.Property;
            var eventInstance = calendarEventProperty.EventInstance;
            var propertyName = $"{nameof(CalendarEvent)}.{property.Name}";

            builder.OpenComponent<InputText>(0);
            builder.AddAttribute(0, "Value", property.GetValue(eventInstance));
            builder.AddAttribute(0, "ValueChanged", CreateInputFieldEventCallback<string>(calendarEventProperty));
            Expression<Func<string>> a = () => propertyName;
            builder.AddAttribute(0, "ValueExpression", a);
            
            builder.CloseComponent();
        };

    private static EventCallback<T> CreateInputFieldEventCallback<T>(CalendarEventProperty eventProperty) where T : class
    {
        return EventCallback.Factory.Create<T>(eventProperty.EventInstance, obj =>
        {
            eventProperty.Property.SetValue(eventProperty.EventInstance, obj);
        });
    }

    private class CalendarEventProperty
    {
        public TCalendarEvent EventInstance { get; init; } = default!;
        public PropertyInfo Property { get; init; } = default!;
    }
}
