using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WindowsIcon
{
    public class VisualToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrameworkElement visual)
            {
                visual.Measure(new Size(visual.Width, visual.Height));
                visual.Arrange(new Rect(0, 0, visual.Width, visual.Height));
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)visual.Width, (int)visual.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(visual);
                return rtb;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}