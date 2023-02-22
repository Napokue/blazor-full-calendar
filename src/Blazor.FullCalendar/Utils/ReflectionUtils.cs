using System.Reflection;

namespace Blazor.FullCalendar.Utils;

internal static class ReflectionUtils
{
    public static bool ContainsAttribute<T>(this Type type) where T : Attribute
    {
        return type.CustomAttributes.Any(a => a.AttributeType == typeof(T));
    }
    
    public static bool ContainsAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
    {
        return propertyInfo.CustomAttributes.Any(a => a.AttributeType == typeof(T));
    }
}

