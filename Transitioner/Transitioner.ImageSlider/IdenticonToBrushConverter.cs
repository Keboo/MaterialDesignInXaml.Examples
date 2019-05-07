using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Jdenticon;

namespace Transitioner.ImageSlider
{
    public class IdenticonToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new VisualBrush(((Identicon) value).ToVisual());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}