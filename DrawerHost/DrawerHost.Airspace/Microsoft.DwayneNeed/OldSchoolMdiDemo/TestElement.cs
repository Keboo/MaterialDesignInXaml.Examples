using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace OldSchoolMdiDemo
{
    public class TestElement : FrameworkElement
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            double rX = RenderSize.Width/2;
            double rY = RenderSize.Height/2;
            Point center = new Point(rX, rY);
            drawingContext.DrawEllipse(Brushes.Red, null, center, rX, rY);
            base.OnRender(drawingContext);
        }
    }
}
