using System.Windows;
using System.Windows.Input;

namespace MDIXWindow
{
    public static class MaterialDesignWindow
    {
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.RegisterAttached(
            "HeaderContent", typeof(object), typeof(MaterialDesignWindow), new PropertyMetadata(default(object)));

        public static void SetHeaderContent(DependencyObject element, object value)
        {
            element.SetValue(HeaderContentProperty, value);
        }

        public static object GetHeaderContent(DependencyObject element)
        {
            return element.GetValue(HeaderContentProperty);
        }

        public static RoutedUICommand Close = new RoutedUICommand();
        public static RoutedUICommand ToggleMaximize = new RoutedUICommand();
        public static RoutedUICommand Minimize = new RoutedUICommand();

        public static void RegisterCommands(Window window)
        {
            window.CommandBindings.Add(new CommandBinding(Close, OnClose));
            window.CommandBindings.Add(new CommandBinding(ToggleMaximize, OnToggleMaximize));
            window.CommandBindings.Add(new CommandBinding(Minimize, OnMinimize));
        }

        public static void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window) window.Close();
        }

        public static void OnToggleMaximize(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Normal
                    ? WindowState.Maximized
                    : WindowState.Normal;
            }
        }

        private static void OnMinimize(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}