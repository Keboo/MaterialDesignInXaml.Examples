using System.Windows;

namespace MDIXWindow
{
    public static class WindowEx
    {
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.RegisterAttached(
            "ShowMinimizeButton", typeof(bool), typeof(WindowEx), new PropertyMetadata(true));

        public static void SetShowMinimizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMinimizeButtonProperty, value);
        }

        public static bool GetShowMinimizeButton(DependencyObject element)
        {
            return (bool) element.GetValue(ShowMinimizeButtonProperty);
        }

        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.RegisterAttached(
            "ShowMaximizeButton", typeof(bool), typeof(WindowEx), new PropertyMetadata(true));

        public static void SetShowMaximizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMaximizeButtonProperty, value);
        }

        public static bool GetShowMaximizeButton(DependencyObject element)
        {
            return (bool) element.GetValue(ShowMaximizeButtonProperty);
        }
    }
}