using Blazor.FullCalendar.Models;

namespace Blazor.FullCalendar.Utils;

internal static class DateUtils
{
    /// <summary>
    /// Calculates the first <see cref="DateOnly"/> of the month based on the input <see cref="DateOnly"/>.  
    /// </summary>
    /// <param name="date"><see cref="DateOnly"/> that is used to calculate the first <see cref="DateOnly"/> of the month with.</param>
    /// <returns>Returns first <see cref="DateOnly"/> of the month.</returns>
    public static DateOnly FirstDayOfMonth(DateOnly date) => date.AddDays(0 - (date.Day - 1));

    
    /// <summary>
    /// Calculates first date of the calendar based on the configured starting day of the week. 
    /// </summary>
    /// <param name="date"><see cref="DateOnly"/> used to calculate the first date of the calendar with.</param>
    /// <returns>Returns the first date of the calendar.</returns>
    public static DateOnly CalculateFirstDateOfCalendar(DayOfWeek StartingDay, DateOnly date, ComponentSharedInformation sharedInformation)
    {
        sharedInformation.FirstDayOfMonth = FirstDayOfMonth(date);
        var dayDifference = CalculateDayDifference(
            sharedInformation.FirstDayOfMonth
                .DayOfWeek, StartingDay);
        return sharedInformation.FirstDayOfMonth.AddDays(0 - dayDifference);
    }
    
    /// <summary>
    /// Calculates the difference between the starting day of the week and the current day.
    /// </summary>
    /// <param name="startingDay">Starting day of the week.</param>
    /// <param name="currentDay">Current day of the week.</param>
    /// <returns>Return the difference between the starting and current day of the week.</returns>
    public static int CalculateDayDifference(
        DayOfWeek currentDay, 
        DayOfWeek startingDay)
    {
        var difference = (int) currentDay - (int) startingDay;

        // If the difference is negative, this means the current day is not in the week of the starting day, but a week before.
        if (difference < 0)
        {
            difference += Constants.AmountOfDisplayedDays;
        }

        return Math.Abs(difference);
    }
}