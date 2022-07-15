using Microsoft.Xaml.Behaviors;
using System.Windows.Input;

namespace TextBox.SelectAllOnDoubleClick;

public class SelectAllBehavior : Behavior<System.Windows.Controls.TextBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MouseDoubleClick += AssociatedObjectOnMouseDoubleClick;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDoubleClick -= AssociatedObjectOnMouseDoubleClick;
        base.OnDetaching();
    }

    private void AssociatedObjectOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        AssociatedObject.SelectAll();
    }
}
