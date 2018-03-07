using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Collections;

namespace Microsoft.DwayneNeed.Controls
{
    public class ComponentPresenter : FrameworkElement
    {
        /// <summary>
        ///     The source Uri of the component to present.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            /* Name:                 */ "Source",
            /* Value Type:           */ typeof(Uri),
            /* Owner Type:           */ typeof(ComponentPresenter),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null,
            /*     Changed Callback: */ (s, e) => ((ComponentPresenter)s).OnSourceChanged(e)));

        /// <summary>
        ///     The source Uri of the component to present.
        /// </summary>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        ///     Return the specified visual child.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (_component == null || index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return _component;
        }

        /// <summary>
        ///     Return the number of visual children.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                if (this._component != null)
                {
                    return 1;
                }
                return 0;
            }
        }

        /// <summary>
        ///     Return the logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (this._component != null)
                {
                    yield return _component;
                }
            }
        }

        /// <summary>
        ///     Measure the loaded component.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            // Load the component if we haven't already.
            if (!_loaded)
            {
                try
                {
                    if (Source != null)
                    {
                        _component = Application.LoadComponent(Source) as FrameworkElement;
                        if (_component != null)
                        {
                            AddVisualChild(_component);
                            AddLogicalChild(_component);
                        }
                    }
                }
                finally
                {
                    _loaded = true;
                }
            }

            // Pass on measure to the child.
            if (_component != null)
            {
                _component.Measure(constraint);
                return _component.DesiredSize;
            }
            else
            {
                return new Size();
            }
        }

        /// <summary>
        ///     Arrange the loaded component.
        /// </summary>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // Pass on arrange to the child.
            if (_component != null)
            {
                _component.Arrange(new Rect(arrangeSize));
            }

            return arrangeSize;
        }

        /// <summary>
        ///     Prepare to load the component from a new source.    
        /// </summary>
        private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_component != null)
            {
                RemoveVisualChild(_component);
                RemoveLogicalChild(_component);
                _component = null;
            }

            _loaded = false;
            InvalidateMeasure();
        }

        private bool _loaded;
        private FrameworkElement _component;
    }
}
