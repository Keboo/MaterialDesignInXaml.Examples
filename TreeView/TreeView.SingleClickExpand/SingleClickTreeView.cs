using System.Windows;

namespace TreeView.SingleClickExpand
{
    public class SingleClickTreeView : System.Windows.Controls.TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SingleClickTreeViewItem();
        }
    }
}