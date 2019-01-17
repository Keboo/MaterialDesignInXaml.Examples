using System.Windows;

namespace DialogHost.CommonDialogWindow
{
    public static class DialogHostEx
    {
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.RegisterAttached(
            "Identifier", typeof(string), typeof(DialogHostEx), new PropertyMetadata(default(string)));

        public static void SetIdentifier(DependencyObject element, string value)
        {
            element.SetValue(IdentifierProperty, value);
        }

        public static string GetIdentifier(DependencyObject element)
        {
            return (string) element.GetValue(IdentifierProperty);
        }
    }
}