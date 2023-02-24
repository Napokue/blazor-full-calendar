using Blazor.FullCalendar.Components.Modals;
using Blazor.FullCalendar.Models;
using Blazor.FullCalendar.Services;
using Blazored.Modal;
using Blazored.Modal.Services;
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

    /// <summary>
    /// The maximum amount of days that need to be displayed in a
    /// month are 31 (max amount of days in a month) + 6 (max amount of deviation between the starting day and first day of the month).
    ///
    /// With 7 * 6 = 42 displayed days the grid supports displaying this.
    /// </summary>
    private const byte AmountOfDisplayedDays = 7;
    private const byte AmountOfDisplayedWeeks = 6;
    
    private readonly DateOnly _today;
    private DateOnly _firstDateOfCalendar;
    private DateOnly _firstDayOfMonth;

    private IReadOnlyList<string> _days = default!;
    private Dictionary<int, TCalendarEvent> _calendarEvents = default!; 

    public FullCalendar()
    {
        _today = DateOnly.FromDateTime(DateTime.Today);
    }

    protected override void OnInitialized()
    {
        _calendarEvents = new Dictionary<int, TCalendarEvent>();
        _firstDateOfCalendar = CalculateFirstDateOfCalendar(_today);
        _days = WeekService.GetDaysBasedOnStartingDay(StartingDay);
    }

    /// <summary>
    /// Calculate the day based on the displayed week and day. 
    /// </summary>
    /// <param name="week">Current displayed week.</param>
    /// <param name="day">Current displayed day.</param>
    /// <returns>Returns the day based on the displayed week and day.</returns>
    private DateOnly CalculateDay(int week, int day) => _firstDateOfCalendar.AddDays(week * AmountOfDisplayedDays + day);
    
    /// <summary>
    /// Calculates first date of the calendar based on the configured starting day of the week. 
    /// </summary>
    /// <param name="date"><see cref="DateOnly"/> used to calculate the first date of the calendar with.</param>
    /// <returns>Returns the first date of the calendar.</returns>
    private DateOnly CalculateFirstDateOfCalendar(DateOnly date)
    {
        _firstDayOfMonth = FirstDayOfMonth(date);
        return _firstDayOfMonth.AddDays(0 - CalculateDayDifference(_firstDayOfMonth.DayOfWeek, StartingDay));
    }

    /// <summary>
    /// Calculates the difference between the starting day of the week and the current day.
    /// </summary>
    /// <param name="startingDay">Starting day of the week.</param>
    /// <param name="currentDay">Current day of the week.</param>
    /// <returns>Return the difference between the starting and current day of the week.</returns>
    private static int CalculateDayDifference(DayOfWeek currentDay, DayOfWeek startingDay)
    {
        var difference = (int) currentDay - (int) startingDay;

        // If the difference is negative, this means the current day is not in the week of the starting day, but a week before.
        if (difference < 0)
        {
            difference += AmountOfDisplayedDays;
        }

        return Math.Abs(difference);
    }

    /// <summary>
    /// Calculates the first <see cref="DateOnly"/> of the month based on the input <see cref="DateOnly"/>.  
    /// </summary>
    /// <param name="date"><see cref="DateOnly"/> that is used to calculate the first <see cref="DateOnly"/> of the month with.</param>
    /// <returns>Returns first <see cref="DateOnly"/> of the month.</returns>
    private static DateOnly FirstDayOfMonth(DateOnly date) => date.AddDays(0 - (date.Day - 1));
    
    private void OnWheel(WheelEventArgs e)
    {
        // Scrolling up
        if (e.DeltaY < 0)
        {
            var date = _firstDayOfMonth.AddMonths(-1);
            _firstDateOfCalendar = CalculateFirstDateOfCalendar(date);
        }

        // Scrolling down
        if (e.DeltaY > 0)
        {
            var date = _firstDayOfMonth.AddMonths(1);
            _firstDateOfCalendar = CalculateFirstDateOfCalendar(date);
        }
    }
    
    private void NavigateToToday()
    {
        _firstDateOfCalendar = CalculateFirstDateOfCalendar(_today);
    }

    private async Task OnDayClick(DateOnly date)
    {
        var calendarEvent = (TCalendarEvent) CalendarEventFactory.Create(date);

        var modalResult = await Modal.Show<CalendarEventModal<TCalendarEvent>>("Event", new ModalParameters
        {
            {"CalendarEvent", calendarEvent},
            {"ButtonCloseText", "Create"}
        }, new ModalOptions
        {
            Size = ModalSize.Medium
        }).Result;
        
        if (modalResult.Confirmed &&
            modalResult.DataType == typeof(TCalendarEvent) &&
            modalResult.Data != null)
        {
            _calendarEvents.TryAdd(date.DayNumber, (TCalendarEvent) modalResult.Data);
        }
    }
    
    private async Task ShowEditModal(TCalendarEvent calendarEvent)
    {
        var editCalendarEvent = (TCalendarEvent) calendarEvent.Clone();
        var modalReference = Modal.Show<CalendarEventModal<TCalendarEvent>>("Event", new ModalParameters
        {
            {"CalendarEvent", editCalendarEvent},
            {"ButtonCloseText", "Save"}
        }, new ModalOptions
        {
            ActivateFocusTrap = true
        });
        var modalResult = await modalReference.Result;

        if (modalResult is {Confirmed: true, Data: not null} && modalResult.DataType == typeof(TCalendarEvent))
        {
            UpdateEvent((TCalendarEvent) modalResult.Data);
        }
    }

    private void UpdateEvent(TCalendarEvent calendarEvent)
    {
        if (!_calendarEvents.ContainsKey(calendarEvent.DayNumber))
        {
            return;
        }
        
        _calendarEvents[calendarEvent.DayNumber] = calendarEvent;
    }

    private void RemoveEvent(TCalendarEvent calendarEvent)
    {
        _calendarEvents.Remove(calendarEvent.DayNumber);
    }

    private string SetMenuDateDisplayInternal() => SetMenuDateDisplay != null
        ? SetMenuDateDisplay(_firstDayOfMonth)
        : _firstDayOfMonth.ToString("MMMM yyyy");

    private string SetFirstDayOfMonthDisplayInternal(DateOnly day) => SetFirstDayOfMonthDisplay != null
        ? SetFirstDayOfMonthDisplay(day)
        : $"{day.ToString("MMM")} {day.Day}";

    private string SetDayDisplayInternal(DateOnly day) => SetDayDisplay != null
        ? SetDayDisplay(day)
        : $"{day.Day}";
}