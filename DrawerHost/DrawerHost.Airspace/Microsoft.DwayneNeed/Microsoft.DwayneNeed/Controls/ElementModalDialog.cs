using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Microsoft.DwayneNeed.Controls
{
    public class ElementModalDialog<V,T> : Adorner where T:IModalContent<V>, new()
    {
        // Private - access through ShowDialog static method.
        private ElementModalDialog(UIElement adornedElement) : base(adornedElement)
        {
        }

        public static V ShowDialog(UIElement adornedElement)
        {
            bool isEnabled = adornedElement.IsEnabled;
            try
            {
                // Disable the adorned element while the dialog is displayed.
                adornedElement.IsEnabled = false;

                ElementModalDialog<V, T> dialog = new ElementModalDialog<V, T>(adornedElement);

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                adornerLayer.Add(dialog);

                T modalContent = new T();
                return modalContent.Accept();

            }
            finally
            {
                // Restore the enabled state of the adorned element once the dialog is dismissed.
                adornedElement.IsEnabled = isEnabled;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(
                Brushes.Blue,
                new Pen(Brushes.Red, 1),
                new Rect(new Point(0, 0), DesiredSize));
            
            base.OnRender(drawingContext);
        }
    }
}
