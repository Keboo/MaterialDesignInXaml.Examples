using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.DwayneNeed.Extensions;

namespace Microsoft.DwayneNeed.MDI
{
    public class MdiPanel : Panel, IScrollInfo
    {
        /// <summary>
        ///     An attached property describing the state of a child of the
        ///     MDIPanel.
        /// </summary>
        public static DependencyProperty WindowStateProperty = DependencyProperty.RegisterAttached(
            /* Name:              */ "WindowState",
            /* Value Type:        */ typeof(WindowState),
            /* Owner Type:        */ typeof(MdiPanel),
            /* Metadata:          */ new FrameworkPropertyMetadata(
            /*     Default Value: */ WindowState.Normal,
            /*     Options:       */ FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        ///     A static getter for the WindowState attached property.
        /// </summary>
        public static WindowState GetWindowState(FrameworkElement e)
        {
            return (WindowState)e.GetValue(WindowStateProperty);
        }

        /// <summary>
        ///     A static setter for the WindowState attached property.
        /// </summary>
        public static void SetWindowState(FrameworkElement e, WindowState value)
        {
            e.SetValue(WindowStateProperty, value);
        }

        /// <summary>
        ///     The location and size of a child of the MDIPanel when it is in
        ///     its normal state.
        /// </summary>
        public static DependencyProperty WindowRectProperty = DependencyProperty.RegisterAttached(
            /* Name:                 */ "WindowRect",
            /* Value Type:           */ typeof(Rect),
            /* Owner Type:           */ typeof(MdiPanel),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ new Rect(),
            /*     Options:          */ FrameworkPropertyMetadataOptions.AffectsParentMeasure,
            /*     Changed Callback: */ (PropertyChangedCallback) OnWindowRectChanged,
            /*     Coerce Callback:  */ CoerceWindowRect));

        /// <summary>
        ///     A static getter for the WindowRect attached property.
        /// </summary>
        public static Rect GetWindowRect(FrameworkElement e)
        {
            return (Rect)e.GetValue(WindowRectProperty);
        }

        /// <summary>
        ///     A static setter for the WindowRect attached property.
        /// </summary>
        public static void SetWindowRect(FrameworkElement e, Rect value)
        {
            e.SetValue(WindowRectProperty, value);
        }

        public static RoutedEvent WindowRectChangedEvent = EventManager.RegisterRoutedEvent(
            /* Name:                */ "WindowRectChanged",
            /* Routing Strategy:    */ RoutingStrategy.Direct,
            /* Handler Type:        */ typeof(RoutedPropertyChangedEventHandler<Rect>),
            /* Owner Type:          */ typeof(MdiPanel));

        public static void AddWindowRectChangedHandler(MdiWindow window, RoutedPropertyChangedEventHandler<Rect> handler)
        {
            window.AddHandler(WindowRectChangedEvent, handler);
        }

        public static void RemoveWindowRectChangedHandler(MdiWindow window, RoutedPropertyChangedEventHandler<Rect> handler)
        {
            window.RemoveHandler(WindowRectChangedEvent, handler);
        }

        /// <summary>
        ///     An internal dependency property key indicating the extents of
        ///     the coordinate space of the panel.
        /// </summary>
        private static DependencyPropertyKey ExtentsPropertyKey = DependencyProperty.RegisterReadOnly(
            /* Name:                 */ "Extents",
            /* Value Type:           */ typeof(Rect),
            /* Owner Type:           */ typeof(MdiPanel),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ new Rect(0, 0, 0, 0),
            /*     Changed Callback: */ (s, e) => ((MdiPanel)s).OnExtentsChanged(e)));

        /// <summary>
        ///     An read-only dependency property indicating the bounds of
        ///     the coordinate space of the panel.
        /// </summary>
        public static DependencyProperty ExtentsProperty = ExtentsPropertyKey.DependencyProperty;

        /// <summary>
        ///     The bounds of the coordinate space of the panel.
        /// </summary>
        public Rect Extents
        {
            get { return (Rect)GetValue(ExtentsProperty); }
            private set { SetValue(ExtentsPropertyKey, value); }
        }

        /// <summary>
        ///     An dependency property indicating the origin of the viewport.
        /// </summary>
        /// <remarks>
        ///     The origin is coerced to be between the left/top edges of the
        ///     extents and the right/bottom edges - viewport size.
        /// </remarks>
        public static DependencyProperty ViewportOriginProperty = DependencyProperty.Register(
            /* Name:                 */ "ViewportOrigin",
            /* Value Type:           */ typeof(Point),
            /* Owner Type:           */ typeof(MdiPanel),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ new Point(0, 0),
            /*     Options:          */ FrameworkPropertyMetadataOptions.AffectsArrange,
            /*     Changed Callback: */ (s, e) => ((MdiPanel)s).OnViewportOriginChanged(e),
            /*     Coerce Callback:  */ (d, bv) => ((MdiPanel)d).CoerceViewportOrigin(bv)));

        /// <summary>
        ///     The origin of the coordinate space of the panel.
        /// </summary>
        public Point ViewportOrigin
        {
            get { return (Point)GetValue(ViewportOriginProperty); }
            set { SetValue(ViewportOriginProperty, value); }
        }

        /// <summary>
        ///     The base (non-coerced) location and size of a child of the
        ///     panel when it is in its normal state.
        /// </summary>
        private static DependencyPropertyKey BaseWindowRectPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            /* Name:                 */ "BaseWindowRect",
            /* Value Type:           */ typeof(Rect),
            /* Owner Type:           */ typeof(MdiPanel),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ new Rect(),
            /*     Options:          */ FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        ///     Determine the desired size of this panel within the specified constraints.
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            Rect bounds = CalculateExtents(availableSize,
                delegate(FrameworkElement child, WindowState windowState, Rect windowRect)
                {
                    if (windowState == WindowState.Normal)
                    {
                        if (!CanHorizontallyScroll)
                        {
                            windowRect.Width = Math.Min(windowRect.Width, availableSize.Width);
                        }

                        if (!CanVerticallyScroll)
                        {
                            windowRect.Height = Math.Min(windowRect.Height, availableSize.Height);
                        }
                    }

                    child.Measure(windowRect.Size);
                });

            // We don't want the framework clipping in our behalf, so we
            // cannot return a desired size larger than the available size.
            Size desiredSize = new Size();
            desiredSize.Width = Math.Min(bounds.Width, availableSize.Width);
            desiredSize.Height = Math.Min(bounds.Height, availableSize.Height);
            return desiredSize;
        }

        /// <summary>
        ///     Arrange the children of this panel.
        /// </summary>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            ViewportWidth = arrangeSize.Width;
            ViewportHeight = arrangeSize.Height;

            Rect extents = new Rect(Origin, arrangeSize);
            if (CanHorizontallyScroll || CanVerticallyScroll)
            {
                double left = extents.Left;
                double top = extents.Top;
                double right = extents.Right;
                double bottom = extents.Bottom;

                // Calculate the natural extents of the panel.
                Rect naturalExtents = CalculateExtents(arrangeSize, (c, ws, wr) => { });

                if (CanHorizontallyScroll)
                {
                    left = Math.Min(extents.Left, naturalExtents.Left);
                    right = Math.Max(extents.Right, naturalExtents.Right);
                }

                if (CanVerticallyScroll)
                {
                    top = Math.Min(extents.Top, naturalExtents.Top);
                    bottom = Math.Max(extents.Bottom, naturalExtents.Bottom);
                }

                extents.X = left;
                extents.Y = top;
                extents.Width = right - left;
                extents.Height = bottom - top;
            }

            Extents = extents;

            // Now that we know the extents, iterate over the children again
            // and arrange them.
            CalculateExtents(arrangeSize,
                delegate(FrameworkElement child, WindowState windowState, Rect windowRect)
                {
                    if (windowState == WindowState.Normal)
                    {
                        // Coerce the WindowRect property on each child to fit
                        // within the extents.
                        child.CoerceValue(WindowRectProperty);
                        windowRect = GetWindowRect(child);

                        // Adjust the position of each child to be relative to
                        // the viewport origin.
                        windowRect.X -= ViewportOrigin.X;
                        windowRect.Y -= ViewportOrigin.Y;
                    }

                    child.Arrange(windowRect);
                });

            // The size of the viewport may have changed, so invalidate the
            // scroll info.
            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            return arrangeSize;
        }

        /// <summary>
        ///     Calculate the ideal bounds of the coordinate space in the
        ///     panel.
        /// </summary>
        private Rect CalculateExtents(Size viewportSize, Action<FrameworkElement, WindowState, Rect> callback)
        {
            double minimizedRowMaxHeight = 0;
            Point minimizedWindowPosition = new Point();
            Rect? minimizedWindowExtents = null;
            Rect? normalWindowExtents = null;

            foreach (FrameworkElement child in ChildrenInZOrder)
            {
                WindowState windowState = GetWindowState(child);
                switch (windowState)
                {
                    case WindowState.Maximized:
                        callback(child, windowState, new Rect(viewportSize));
                        break;

                    case WindowState.Minimized:
                        Size minimumSize = new Size(child.MinWidth, child.MinHeight);

                        // Wrap to the next row if the child doesn't fit in this row.
                        if (minimizedWindowPosition.X + minimumSize.Width > viewportSize.Width)
                        {
                            minimizedWindowPosition.X = 0;
                            minimizedWindowPosition.Y += minimizedRowMaxHeight;
                            minimizedRowMaxHeight = 0;
                        }

                        Rect minimizedWindowRect = new Rect(minimizedWindowPosition, minimumSize);
                        callback(child, windowState, minimizedWindowRect);

                        if (minimizedWindowExtents.HasValue)
                        {
                            Rect temp = minimizedWindowExtents.Value;
                            temp.Union(minimizedWindowRect);
                            minimizedWindowExtents = temp;
                        }
                        else
                        {
                            minimizedWindowExtents = minimizedWindowRect;
                        }

                        // Increment the horizontal location for the next child.  We
                        // will wrap vertically once we place the next child.
                        minimizedWindowPosition.X += minimumSize.Width;
                        minimizedRowMaxHeight = Math.Max(minimizedRowMaxHeight, minimumSize.Height);
                        break;

                    case WindowState.Normal:
                    default:
                        Rect windowRect = (Rect)child.GetValue(MdiPanel.BaseWindowRectPropertyKey.DependencyProperty);

                        // Respect minimum size.
                        windowRect.Width = Math.Max(windowRect.Width, child.MinWidth);
                        windowRect.Height = Math.Max(windowRect.Height, child.MinHeight);

                        callback(child, windowState, windowRect);

                        if (normalWindowExtents.HasValue)
                        {
                            Rect temp = normalWindowExtents.Value;
                            temp.Union(windowRect);
                            normalWindowExtents = temp;
                        }
                        else
                        {
                            normalWindowExtents = windowRect;
                        }

                        break;
                }
            }

            Rect extents = normalWindowExtents ?? new Rect(0, 0, 0, 0);

            // The minimized windows will be arranged in the viewport, so
            // reserve space for them (but location doesn't matter).
            if (minimizedWindowExtents.HasValue)
            {
                extents.Union(new Rect(extents.TopLeft, minimizedWindowExtents.Value.Size));
            }

            return extents;
        }

        private IEnumerable<FrameworkElement> ChildrenInZOrder
        {
            get
            {
                // 1. Upcast to FrameworkElement
                // 2. Ensure against null values in the children collection,
                //    which are technically possible (though rare)
                // 3. Order by ZIndex
                return Children.OfType<FrameworkElement>().Where((e) => e != null).OrderBy((e) => Panel.GetZIndex(e));
            }
        }

        private static object CoerceWindowRect(DependencyObject d, object baseValue)
        {
            MdiPanel panel = VisualTreeHelper.GetParent(d) as MdiPanel;
            FrameworkElement child = d as FrameworkElement;
            Rect windowRect = (Rect)baseValue;

            if (panel != null && d != null)
            {
                // Record the base value since we will be using for internal
                // calculations.
                d.SetValue(MdiPanel.BaseWindowRectPropertyKey, windowRect);

                // The window rect size must at least be as large as the
                // child's minimum size.
                windowRect.Width = Math.Max(windowRect.Width, child.MinWidth);
                windowRect.Height = Math.Max(windowRect.Height, child.MinHeight);

                // The window rect is constrained to the panel bounds.
                windowRect = windowRect.ConstrainWithin(panel.Extents);
            }

            return windowRect;
        }

        private void OnExtentsChanged(DependencyPropertyChangedEventArgs e)
        {
            // When the extents change, re-coerce the viewport origin.
            // We actually set the local value to avoid the origin getting
            // left behind.
            ViewportOrigin = (Point)CoerceViewportOrigin(ViewportOrigin);

            // The scroll owner needs to know that scrolling information has
            // changed.
            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }
        }

        private object CoerceViewportOrigin(object baseValue)
        {
            Point viewportOrigin = (Point)baseValue;

            viewportOrigin.X = Math.Min(Math.Max(viewportOrigin.X, Extents.Left), Extents.Right - ViewportWidth);
            viewportOrigin.Y = Math.Min(Math.Max(viewportOrigin.Y, Extents.Top), Extents.Bottom - ViewportHeight);

            return viewportOrigin;
        }

        private void OnViewportOriginChanged(DependencyPropertyChangedEventArgs e)
        {
            // The scroll owner needs to know that scrolling information has
            // changed.
            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }
        }

        public ScrollViewer ScrollOwner { get; set; }
        public bool CanHorizontallyScroll { get; set; }
        public bool CanVerticallyScroll { get; set; }

        public double ViewportWidth { get; private set; }
        public double ViewportHeight { get; private set; }

        public double ExtentWidth
        {
            get
            {
                return Extents.Width;
            }
        }

        public double ExtentHeight
        {
            get
            {
                return Extents.Height;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return ViewportOrigin.X - Extents.Left;
            }
        }

        public double VerticalOffset
        {
            get
            {
                return ViewportOrigin.Y - Extents.Top;
            }
        }

        public void LineLeft()
        {
            SetHorizontalOffset(HorizontalOffset - LineSize);
        }

        public void LineRight()
        {
            SetHorizontalOffset(HorizontalOffset + LineSize);
        }

        public void LineUp()
        {
            SetVerticalOffset(VerticalOffset - LineSize);
        }

        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + LineSize);
        }

        public void MouseWheelLeft()
        {
            SetHorizontalOffset(HorizontalOffset - WheelSize);
        }

        public void MouseWheelRight()
        {
            SetHorizontalOffset(HorizontalOffset + WheelSize);
        }

        public void MouseWheelUp()
        {
            SetVerticalOffset(VerticalOffset - WheelSize);
        }

        public void MouseWheelDown()
        {
            SetVerticalOffset(VerticalOffset + WheelSize);
        }

        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - ViewportWidth);
        }

        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + ViewportWidth);
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - ViewportHeight);
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + ViewportHeight);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            // Translate the rect from the specified visual up to this panel.
            rectangle = ((UIElement)visual).TransformElementToElement(rectangle, this);

            // Translate the rect into the extents coordinates.
            rectangle.Offset(new Vector(ViewportOrigin.X, ViewportOrigin.Y));

            Point viewportOrigin = ViewportOrigin;
            if (rectangle.Right > (viewportOrigin.X + ViewportWidth))
            {
                viewportOrigin.X = rectangle.Right - ViewportWidth;
            }
            if (rectangle.Left < viewportOrigin.X)
            {
                viewportOrigin.X = rectangle.Left;
            }

            if (rectangle.Bottom > (viewportOrigin.Y + ViewportHeight))
            {
                viewportOrigin.Y = rectangle.Bottom - ViewportHeight;
            }
            if (rectangle.Top < viewportOrigin.Y)
            {
                viewportOrigin.Y = rectangle.Top;
            }

            ViewportOrigin = viewportOrigin;

            return rectangle;
        }

        public void SetHorizontalOffset(double offset)
        {
            Point viewportOrigin = new Point(Extents.Left + offset, ViewportOrigin.Y);

            ViewportOrigin = viewportOrigin;
        }

        public void SetVerticalOffset(double offset)
        {
            Point viewportOrigin = new Point(ViewportOrigin.X, Extents.Top + offset);

            ViewportOrigin = viewportOrigin;
        }

        private static void OnWindowRectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MdiWindow window = d as MdiWindow;
            if(window != null)
            {
                RoutedPropertyChangedEventArgs<Rect> args = new RoutedPropertyChangedEventArgs<Rect>((Rect)e.OldValue, (Rect)e.NewValue, WindowRectChangedEvent);
                window.RaiseEvent(args);
            }
        }

        // Could make this a DP if we wanted to.
        private const double LineSize = 16;
        private const double WheelSize = 3 * LineSize;
        private readonly static Point Origin = new Point(0, 0);
    }
}
