using Blazor.FullCalendar.Models;
using Blazor.FullCalendar.Services;
using Blazor.FullCalendar.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.FullCalendar.Components;

public partial class FullCalendar<TCalendarEvent> : ComponentBase where TCalendarEvent : ICalendarEvent
{
    [Parameter] public bool EnableMenu { get; set; } = true;
    [Parameter] public bool EnableDistinctFirstDayOfMonth { get; set; } = true;
    [Parameter] public bool EnableHighlightToday { get; set; } = true;
    [Parameter] public Func<DateOnly, string>? SetMenuDateDisplay { get; set; }
    [Parameter] public Func<DateOnly, string>? SetFirstDayOfMonthDisplay { get; set; }
    [Parameter] public Func<DateOnly, string>? SetDayDisplay { get; set; }
    [Parameter] public DayOfWeek StartingDay { get; set; } = DayOfWeek.Sunday;

    public readonly ComponentSharedInformation ComponentSharedInformation;

    private IReadOnlyList<string>? _days;
    private Dictionary<int, TCalendarEvent>? _calendarEvents; 

    public FullCalendar()
    {
        ComponentSharedInformation = new ComponentSharedInformation();
    }

    protected override void OnInitialized()
    {
        ComponentSharedInformation.FirstDateOfCalendar = CalculateFirstDateOfCalendar(ComponentSharedInformation.Today);
        
        // TODO Review how this can be improved, StateHasChanged is called too much
        ComponentSharedInformation.PropertyChanged += (_, _) => StateHasChanged();
        _calendarEvents = new Dictionary<int, TCalendarEvent>();
        _days = WeekService.GetDaysBasedOnStartingDay(StartingDay);
    }

    /// <summary>
    /// Calculate the day based on the displayed week and day. 
    /// </summary>
    /// <param name="week">Current displayed week.</param>
    /// <param name="day">Current displayed day.</param>
    /// <returns>Returns the day based on the displayed week and day.</returns>
    private DateOnly CalculateDay(int week, int day) =>
        ComponentSharedInformation.FirstDateOfCalendar.AddDays(week * Constants.AmountOfDisplayedDays + day);
    
    /// <summary>
    /// Calculates first date of the calendar based on the configured starting day of the week. 
    /// </summary>
    /// <param name="date"><see cref="DateOnly"/> used to calculate the first date of the calendar with.</param>
    /// <returns>Returns the first date of the calendar.</returns>
    private DateOnly CalculateFirstDateOfCalendar(DateOnly date)
    {
        ComponentSharedInformation.FirstDayOfMonth = DateUtils.FirstDayOfMonth(date);
        var dayDifference = DateUtils.CalculateDayDifference(
            ComponentSharedInformation.FirstDayOfMonth
                .DayOfWeek, StartingDay);
        return ComponentSharedInformation.FirstDayOfMonth.AddDays(0 - dayDifference);
    }
    
    private void OnWheel(WheelEventArgs e)
    {
        // Scrolling up
        if (e.DeltaY < 0)
        {
            var date = ComponentSharedInformation.FirstDayOfMonth.AddMonths(-1);
            ComponentSharedInformation.FirstDateOfCalendar = CalculateFirstDateOfCalendar(date);
        }

        // Scrolling down
        if (e.DeltaY > 0)
        {
            var date = ComponentSharedInformation.FirstDayOfMonth.AddMonths(1);
            ComponentSharedInformation.FirstDateOfCalendar = CalculateFirstDateOfCalendar(date);
        }
    }
    
    private void OnDayClick(DateOnly date)
    {
        _calendarEvents.TryAdd(date.DayNumber, (TCalendarEvent) CalendarEventFactory.Create(date));
    }

    

    private string SetFirstDayOfMonthDisplayInternal(DateOnly day) => SetFirstDayOfMonthDisplay != null
        ? SetFirstDayOfMonthDisplay(day)
        : $"{day.ToString("MMM")} {day.Day}";

    private string SetDayDisplayInternal(DateOnly day) => SetDayDisplay != null
        ? SetDayDisplay(day)
        : $"{day.Day}";
}