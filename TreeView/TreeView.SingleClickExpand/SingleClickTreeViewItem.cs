using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeView.SingleClickExpand
{
    public class SingleClickTreeViewItem : TreeViewItem
    {
        static SingleClickTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleClickTreeViewItem), new FrameworkPropertyMetadata(typeof(SingleClickTreeViewItem)));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            bool handled = e.Handled;
            base.OnMouseLeftButtonDown(e);

            if (!handled && IsEnabled)
            {
                if (e.ClickCount == 1)
                {
                    SetCurrentValue(IsExpandedProperty, !IsExpanded);
                    e.Handled = true;
                }
            }
        }
    }
}