using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Blazor.FullCalendar.Models;

public class ComponentSharedInformation : INotifyPropertyChanged
{
    public readonly DateOnly Today;

    public DateOnly FirstDateOfCalendar
    {
        get => _firstDateOfCalendar;
        set => SetProperty(ref _firstDateOfCalendar, value);
    }
    public DateOnly FirstDayOfMonth
    {
        get => _firstDayOfMonth;
        set => SetProperty(ref _firstDayOfMonth, value);
    }

    private DateOnly _firstDateOfCalendar;
    private DateOnly _firstDayOfMonth;

    public ComponentSharedInformation()
    {
        Today = DateOnly.FromDateTime(DateTime.Today);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}