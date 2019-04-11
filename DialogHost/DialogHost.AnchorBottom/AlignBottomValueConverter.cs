using System;
using System.Globalization;
using System.Windows.Data;

namespace DialogHost.AnchorBottom
{

    public class AlignBottomValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2 &&
                values[0] is double dialogHostHeight &&
                values[1] is double dialogHeight &&
                IsValid(dialogHostHeight) &&
                IsValid(dialogHeight))
            {
                //Dialog will be centered
                return (dialogHostHeight - dialogHeight) / 2;
            }
            return Binding.DoNothing;

            bool IsValid(double value) => !double.IsInfinity(value) && !double.IsNaN(value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}