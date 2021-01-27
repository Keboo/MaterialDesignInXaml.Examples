using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace SetFocusBehavior
{
    public class FocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.IsVisibleChanged += AssociatedObjectOnIsVisibleChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsVisibleChanged -= AssociatedObjectOnIsVisibleChanged;
            base.OnDetaching();
        }

        private void AssociatedObjectOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AssociatedObject.Focus();
        }
    }
}