using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVM.TextEditor.Converters;

public class NullToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
