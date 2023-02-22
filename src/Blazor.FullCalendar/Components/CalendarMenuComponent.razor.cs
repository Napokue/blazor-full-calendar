using Blazor.FullCalendar.Models;
using Blazor.FullCalendar.Utils;
using Microsoft.AspNetCore.Components;

namespace Blazor.FullCalendar.Components;

public partial class CalendarMenuComponent
{
    [Parameter] public Func<DateOnly, string>? SetMenuDateDisplay { get; set; }
    [Parameter] public DayOfWeek StartingDay { get; set; }
    [CascadingParameter] public ComponentSharedInformation ComponentSharedInformation { get; set; } = default!;

    public void NavigateToToday()
    {
        ComponentSharedInformation.FirstDateOfCalendar =
            DateUtils.CalculateFirstDateOfCalendar(StartingDay, ComponentSharedInformation.Today,
                ComponentSharedInformation);
    }

    private string SetMenuDateDisplayInternal() => SetMenuDateDisplay != null
        ? SetMenuDateDisplay(ComponentSharedInformation.FirstDayOfMonth)
        : ComponentSharedInformation.FirstDayOfMonth.ToString("MMMM yyyy");
}