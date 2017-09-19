using System.Windows;

namespace DragOverConditional
{
    public static class Drag
    {
        public static readonly DependencyProperty IsDragOverProperty = DependencyProperty.RegisterAttached(
            "IsDragOver", typeof(bool), typeof(Drag), new PropertyMetadata(default(bool)));

        public static void SetIsDragOver(DependencyObject element, bool value)
        {
            element.SetValue(IsDragOverProperty, value);
        }

        public static bool GetIsDragOver(DependencyObject element)
        {
            return (bool) element.GetValue(IsDragOverProperty);
        }
    }
}