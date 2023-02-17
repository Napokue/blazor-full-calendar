using Microsoft.AspNetCore.Components;

namespace Blazor.FullCalendar;

public partial class FullCalendar : ComponentBase
{
    private const byte AmountOfDisplayedDays = 7;
    private const byte AmountOfDisplayedWeeks = 5;
    
    private readonly DateTime _today;
    private readonly DateOnly _firstDateOfCalendar;

    public FullCalendar()
    {
        _today = DateTime.Today;
        _firstDateOfCalendar = CalculateFirstDateOfCalendar(_today);
    }

    /// <summary>
    /// Calculate the day based on the displayed week and day. 
    /// </summary>
    /// <param name="week">Current displayed week.</param>
    /// <param name="day">Current displayed day.</param>
    /// <returns>Returns the day based on the displayed week and day.</returns>
    private int CalculateDay(int week, int day) => _firstDateOfCalendar.AddDays(week * AmountOfDisplayedDays + day).Day;
    
    /// <summary>
    /// Calculates first date of the calendar based on the configured starting day of the week. 
    /// </summary>
    /// <param name="dateTime"><see cref="DateTime"/> used to calculate the first date of the calendar with.</param>
    /// <returns>Returns the first date of the calendar.</returns>
    private DateOnly CalculateFirstDateOfCalendar(DateTime dateTime)
    {
        var firstDayOfMonth = FirstDayOfMonth(dateTime);
        return DateOnly.FromDateTime(
            firstDayOfMonth.Subtract(
                TimeSpan.FromDays(CalculateDayDifference(DayOfWeek.Sunday, firstDayOfMonth.DayOfWeek))));
    }

    /// <summary>
    /// Calculates the difference between the starting day of the week and the current day.
    /// </summary>
    /// <param name="startingDay">Starting day of the week.</param>
    /// <param name="currentDay">Current day of the week.</param>
    /// <returns>Return the difference between the starting and current day of the week.</returns>
    private static int CalculateDayDifference(DayOfWeek startingDay, DayOfWeek currentDay) =>
        Math.Abs((int) startingDay - (int) currentDay);
    
    /// <summary>
    /// Calculates the first <see cref="DateTime"/> of the month based on the input <see cref="DateTime"/>.  
    /// </summary>
    /// <param name="dateTime"><see cref="DateTime"/> that is used to calculate the first <see cref="DateTime"/> of the month with.</param>
    /// <returns>Returns first <see cref="DateTime"/> of the month.</returns>
    private DateTime FirstDayOfMonth(DateTime dateTime) => _today.Subtract(TimeSpan.FromDays(_today.Day - 1));
}