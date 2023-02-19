using Blazor.FullCalendar.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.FullCalendar;

public partial class FullCalendar : ComponentBase
{
    [Parameter] public bool EnableMenu { get; set; } = true;
    [Parameter] public Func<DateTime, string>? SetMenuDateDisplay { get; set; }

    private const DayOfWeek StartingDay = DayOfWeek.Sunday;
    
    /// <summary>
    /// The maximum amount of days that need to be displayed in a
    /// month are 31 (max amount of days in a month) + 6 (max amount of deviation between the starting day and first day of the month).
    ///
    /// With 7 * 6 = 42 displayed days the grid supports displaying this.
    /// </summary>
    private const byte AmountOfDisplayedDays = 7;
    private const byte AmountOfDisplayedWeeks = 6;
    
    private readonly DateTime _today;
    private DateOnly _firstDateOfCalendar;
    private readonly IReadOnlyList<string> _days;
    private DateTime _firstDayOfMonth;

    public FullCalendar()
    {
        _today = DateTime.Today;
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
    /// <param name="dateTime"><see cref="DateTime"/> used to calculate the first date of the calendar with.</param>
    /// <returns>Returns the first date of the calendar.</returns>
    private DateOnly CalculateFirstDateOfCalendar(DateTime dateTime)
    {
        _firstDayOfMonth = FirstDayOfMonth(dateTime);
        return DateOnly.FromDateTime(
            _firstDayOfMonth.Subtract(
                TimeSpan.FromDays(CalculateDayDifference(_firstDayOfMonth.DayOfWeek, StartingDay))));
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
    /// Calculates the first <see cref="DateTime"/> of the month based on the input <see cref="DateTime"/>.  
    /// </summary>
    /// <param name="dateTime"><see cref="DateTime"/> that is used to calculate the first <see cref="DateTime"/> of the month with.</param>
    /// <returns>Returns first <see cref="DateTime"/> of the month.</returns>
    private DateTime FirstDayOfMonth(DateTime dateTime) => dateTime.Subtract(TimeSpan.FromDays(dateTime.Day - 1));
    
    private void OnWheel(WheelEventArgs e)
    {
        // Scrolling up
        if (e.DeltaY < 0)
        {
            var dateTime = _firstDayOfMonth.AddMonths(-1);
            _firstDateOfCalendar = CalculateFirstDateOfCalendar(dateTime);
        }

        // Scrolling down
        if (e.DeltaY > 0)
        {
            var dateTime = _firstDayOfMonth.AddMonths(1);
            _firstDateOfCalendar = CalculateFirstDateOfCalendar(dateTime);
        }
    }
    
    private void NavigateToToday()
    {
        _firstDateOfCalendar = CalculateFirstDateOfCalendar(_today);
    }

    private string SetMenuDateDisplayInternal() => SetMenuDateDisplay != null
        ? SetMenuDateDisplay(_firstDayOfMonth)
        : _firstDayOfMonth.ToString("MMMM yyyy");
}