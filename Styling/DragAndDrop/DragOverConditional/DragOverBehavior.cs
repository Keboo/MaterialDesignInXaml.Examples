using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace DragOverConditional
{
    public class DragOverBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragEnter += AssociatedObjectOnDragOver;
            AssociatedObject.DragLeave += AssociatedObjectOnDragOff;
            AssociatedObject.Drop += AssociatedObjectOnDragOff;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.DragEnter -= AssociatedObjectOnDragOver;
            AssociatedObject.DragLeave -= AssociatedObjectOnDragOff;
            AssociatedObject.Drop -= AssociatedObjectOnDragOff;
            base.OnDetaching();
        }

        private void AssociatedObjectOnDragOver(object sender, DragEventArgs dragEventArgs)
        {
            //TODO: Any sort of filtering that you might want to do
            //Consider using the methods dragEventArgs.Data to check for particular types of data
            Drag.SetIsDragOver(AssociatedObject, true);
        }

        private void AssociatedObjectOnDragOff(object sender, DragEventArgs dragEventArgs)
        {
            Drag.SetIsDragOver(AssociatedObject, false);
        }
    }
}