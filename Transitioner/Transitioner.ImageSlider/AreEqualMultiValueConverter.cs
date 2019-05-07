using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Transitioner.ImageSlider
{
    public class AreEqualMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 1)
            {
                return values.Skip(1).All(x => Equals(x, values[0]));
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}