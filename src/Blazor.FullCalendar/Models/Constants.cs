namespace Blazor.FullCalendar.Models;

internal static class Constants
{
    public const string NoTitle = "No Title";
    
    /// <summary>
    /// The maximum amount of days that need to be displayed in a
    /// month are 31 (max amount of days in a month) + 6 (max amount of deviation between the starting day and first day of the month).
    ///
    /// With 7 * 6 = 42 displayed days the grid supports displaying this.
    /// </summary>
    public const byte AmountOfDisplayedDays = 7;
    public const byte AmountOfDisplayedWeeks = 6;
}