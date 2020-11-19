using MaterialDesignThemes.Wpf;
using System.Windows;

namespace ExtendTextBoxTemplate
{
    public static class TextBoxEx
    {
        public static PackIconKind GetIcon(DependencyObject obj)
        {
            return (PackIconKind)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, PackIconKind value)
        {
            obj.SetValue(IconProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(PackIconKind), typeof(TextBoxEx), 
                new PropertyMetadata(PackIconKind.None));


    }
}
