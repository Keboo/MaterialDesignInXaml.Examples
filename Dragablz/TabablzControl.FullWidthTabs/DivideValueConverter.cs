using System;
using System.Globalization;
using System.Windows.Data;

namespace TabablzControl.FullWidthTabs
{
    public class DivideValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
