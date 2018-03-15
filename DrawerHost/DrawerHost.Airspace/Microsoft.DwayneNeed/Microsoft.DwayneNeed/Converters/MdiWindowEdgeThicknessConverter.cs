using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.MDI;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Microsoft.DwayneNeed.Converters
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class MdiWindowEdgeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            MdiWindowEdge edges = (MdiWindowEdge)Enum.Parse(typeof(MdiWindowEdge), (string)parameter);

            if ((edges & MdiWindowEdge.Left) == 0)
            {
                thickness.Left = 0;
            }
            if ((edges & MdiWindowEdge.Top) == 0)
            {
                thickness.Top = 0;
            }
            if ((edges & MdiWindowEdge.Right) == 0)
            {
                thickness.Right = 0;
            }
            if ((edges & MdiWindowEdge.Bottom) == 0)
            {
                thickness.Bottom = 0;
            }
            return thickness;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
