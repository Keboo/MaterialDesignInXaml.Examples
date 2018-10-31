using System.Windows;
using System.Windows.Media;

namespace MVVM.Validation
{
    public static class ResourcesEx
    {
        public static readonly DependencyProperty PrimaryHueMidBrushProperty = DependencyProperty.RegisterAttached(
            "PrimaryHueMidBrush", typeof(Brush), typeof(ResourcesEx), new PropertyMetadata(default(Brush), OnPrimaryHueMidBrushChanged));

        private static void OnPrimaryHueMidBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.NewValue is Brush newBrush)
                {
                    element.Resources.Add("PrimaryHueMidBrush", newBrush);
                }
                else
                {
                    element.Resources.Remove("PrimaryHueMidBrush");
                }
            }
        }

        public static void SetPrimaryHueMidBrush(DependencyObject element, Brush value)
        {
            element.SetValue(PrimaryHueMidBrushProperty, value);
        }

        public static Brush GetPrimaryHueMidBrush(DependencyObject element)
        {
            return (Brush) element.GetValue(PrimaryHueMidBrushProperty);
        }
    }
}