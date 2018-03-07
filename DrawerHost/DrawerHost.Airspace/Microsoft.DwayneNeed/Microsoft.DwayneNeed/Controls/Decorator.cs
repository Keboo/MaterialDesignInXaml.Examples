using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Microsoft.DwayneNeed.Controls
{
    [ContentProperty("Child")]
    public class Decorator<T> : FrameworkElement where T : FrameworkElement
    {
        public static DependencyProperty ChildProperty = DependencyProperty.Register(
            /* Name:                 */ "Child",
            /* Value Type:           */ typeof(FrameworkElement),
            /* Owner Type:           */ typeof(Decorator<T>),
            /* Metadata:             */ new PropertyMetadata(
            /*     Default Value:    */ null,
            /*     Property Changed: */ (d, e) => ((Decorator<T>)d).OnChildChanged(e)));

        public T Child
        {
            get { return (T)GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        protected virtual void OnChildChanged(T oldChild, T newChild)
        {
        }

        private void OnChildChanged(DependencyPropertyChangedEventArgs e)
        {
            T oldChild = (T) e.OldValue;
            if (oldChild != null)
            {
                base.RemoveVisualChild(oldChild);
                base.RemoveLogicalChild(oldChild);
            }

            T newChild = (T) e.NewValue;
            if (newChild != null)
            {
                AddLogicalChild(newChild);
                AddVisualChild(newChild);
            }

            InvalidateMeasure();

            OnChildChanged(oldChild, newChild);
        }

        protected override System.Collections.IEnumerator LogicalChildren
        {
            get
            {
                T child = Child;
                if (child != null)
                {
                    yield return child;
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return (Child != null) ? 1 : 0;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            T child = Child;
            if ((child == null) || (index != 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            T child = Child;
            if (child != null)
            {
                child.Measure(constraint);
                return child.DesiredSize;
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            T child = Child;
            if (child != null)
            {
                child.Arrange(new Rect(arrangeSize));
            }
            return arrangeSize;
        }
    }
}
