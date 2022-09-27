using System.Windows;

namespace TabControl.MVVM;

public class CachingTabControl : System.Windows.Controls.TabControl
{
    protected override DependencyObject GetContainerForItemOverride()
    {
        return base.GetContainerForItemOverride();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return base.IsItemItsOwnContainerOverride(item);
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        base.ClearContainerForItemOverride(element, item);
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);
    }
}