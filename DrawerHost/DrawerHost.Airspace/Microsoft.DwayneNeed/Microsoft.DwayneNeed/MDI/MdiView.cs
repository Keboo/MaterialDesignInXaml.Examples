using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.DwayneNeed.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.DwayneNeed.Extensions;
using System.Windows.Interop;
using Microsoft.DwayneNeed.Interop;

namespace Microsoft.DwayneNeed.MDI
{
    public class MdiView : SelectorEx<MdiWindow>
    {
        #region ContentAirspaceMode
        public static readonly DependencyProperty ContentAirspaceModeProperty = DependencyProperty.Register(
            /* Name:                */ "ContentAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiView),
            /* Metadata:            */ new FrameworkPropertyMetadata(
            /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     Whether or not the content should be clipped.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an HwndClipper to enforce this property.
        /// </remarks>
        public AirspaceMode ContentAirspaceMode
        {
            get { return (AirspaceMode)GetValue(ContentAirspaceModeProperty); }
            set { SetValue(ContentAirspaceModeProperty, value); }
        }
        #endregion
        #region ContentClippingBackground
        public static readonly DependencyProperty ContentClippingBackgroundProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentClippingBackground",
            /* Value Type:           */ typeof(Brush),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public Brush ContentClippingBackground
        {
            get { return (Brush)GetValue(ContentClippingBackgroundProperty); }
            set { SetValue(ContentClippingBackgroundProperty, value); }
        }
        #endregion
        #region ContentClippingCopyBitsBehavior
        public static readonly DependencyProperty ContentClippingCopyBitsBehaviorProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior ContentClippingCopyBitsBehavior
        {
            get { return (CopyBitsBehavior)GetValue(ContentClippingCopyBitsBehaviorProperty); }
            set { SetValue(ContentClippingCopyBitsBehaviorProperty, value); }
        }
        #endregion
        #region ContentRedirectionVisibility
        public static readonly DependencyProperty ContentRedirectionVisibilityProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility ContentRedirectionVisibility
        {
            get { return (RedirectionVisibility)GetValue(ContentRedirectionVisibilityProperty); }
            set { SetValue(ContentRedirectionVisibilityProperty, value); }
        }
        #endregion
        #region ContentIsOutputRedirectionEnabled
        public static readonly DependencyProperty ContentIsOutputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool ContentIsOutputRedirectionEnabled
        {
            get { return (bool)GetValue(ContentIsOutputRedirectionEnabledProperty); }
            set { SetValue(ContentIsOutputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region ContentOutputRedirectionPeriod
        public static readonly DependencyProperty ContentOutputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan ContentOutputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(ContentOutputRedirectionPeriodProperty); }
            set { SetValue(ContentOutputRedirectionPeriodProperty, value); }
        }
        #endregion
        #region ContentIsInputRedirectionEnabled
        public static readonly DependencyProperty ContentIsInputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool ContentIsInputRedirectionEnabled
        {
            get { return (bool)GetValue(ContentIsInputRedirectionEnabledProperty); }
            set { SetValue(ContentIsInputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region ContentInputRedirectionPeriod
        public static readonly DependencyProperty ContentInputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan ContentInputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(ContentInputRedirectionPeriodProperty); }
            set { SetValue(ContentInputRedirectionPeriodProperty, value); }
        }
        #endregion

        #region EnableSnapping
        /// <summary>
        ///     A dependency property indicating whether or not the window
        ///     rect of a window in the view should be snapped to the
        ///     edges of the other children during interactive operations.
        /// </summary>
        public static DependencyProperty EnableSnappingProperty = DependencyProperty.Register(
            /* Name:              */ "EnableSnapping",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiView),
            /* Metadata:          */ new FrameworkPropertyMetadata(
            /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the window rect of a window in the view
        ///     should be snapped to the edges of the other children during
        ///     interactive operations.
        /// </summary>
        public bool EnableSnapping
        {
            get { return (bool)GetValue(EnableSnappingProperty); }
            set { SetValue(EnableSnappingProperty, value); }
        }
        #endregion
        #region SnapThreshold
        /// <summary>
        ///     A dependency property indicating the threshold to use when
        ///     snapping the window rect of a window in the view.
        /// </summary>
        public static DependencyProperty SnapThresholdProperty = DependencyProperty.Register(
            /* Name:                */ "SnapThreshold",
            /* Value Type:          */ typeof(double),
            /* Owner Type:          */ typeof(MdiView),
            /* Metadata:            */ new FrameworkPropertyMetadata(
            /*     Default Value:   */ 10.0));

        /// <summary>
        ///     The threshold to use when snapping the window rect of a
        ///     window in the view.
        /// </summary>
        public double SnapThreshold
        {
            get { return (double)GetValue(SnapThresholdProperty); }
            set { SetValue(SnapThresholdProperty, value); }
        }
        #endregion

        static MdiView()
        {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiView), new FrameworkPropertyMetadata(typeof(MdiView)));

            Selector.SelectedItemProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiView),
                /* Metadata:             */ new FrameworkPropertyMetadata(
                /*     Changed Callback: */ (s, e) => ((MdiView)s).OnSelectedItemChanged(e),
                /*     Coerce Callback:  */ (d, bv) => ((MdiView)d).OnCoerceSelectedItem(bv)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MaximizeWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteMaximizeWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteMaximizeWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MinimizeWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteMinimizeWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteMinimizeWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.RestoreWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteRestoreWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteRestoreWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.CloseWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteCloseWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteCloseWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.AdjustWindowRect,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteAdjustWindowRect(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteAdjustWindowRect(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.ActivateWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteActivateWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteActivateWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.ActivateNextWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteActivateNextWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            CommandManager.RegisterClassInputBinding(
                /* Type:            */ typeof(MdiView),
                /* Input Binding:   */ new InputBinding(
                /*     Command:     */ MdiCommands.ActivateNextWindow,
                /*     Gesture:     */ new KeyGesture(Key.Tab, ModifierKeys.Control)));

            CommandManager.RegisterClassInputBinding(
                /* Type:            */ typeof(MdiView),
                /* Input Binding:   */ new InputBinding(
                /*     Command:     */ MdiCommands.ActivatePreviousWindow,
                /*     Gesture:     */ new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.ActivatePreviousWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteActivatePreviousWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.TileWindows,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteTileWindows(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.CascadeWindows,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteCascadeWindows(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MinimizeAllWindows,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteMinimizeAllWindows(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteMinimizeAllWindows(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MaximizeAllWindows,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteMaximizeAllWindows(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteMaximizeAllWindows(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.RestoreAllWindows,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteRestoreAllWindows(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteRestoreAllWindows(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.FloatWindow,
                /*     Execute:     */ (s, e) => ((MdiView)s).ExecuteFloatWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiView)s).CanExecuteFloatWindow(e)));
        }

        /// <summary>
        ///     Constructor for the MdiView class.
        /// </summary>
        public MdiView()
        {
            Windows = new ReadOnlyObservableCollection<MdiWindow>(_windows);
        }

        /// <summary>
        ///     A read-only observable collection of all windows in this view.
        /// </summary>
        public ReadOnlyObservableCollection<MdiWindow> Windows { get; private set; }

        /// <summary>
        ///     Returns the effective ZIndex for a window in the view.
        /// </summary>
        /// <remarks>
        ///     The MdiWindow control will coerce its ZOrder property
        ///     to this value.
        /// </remarks>
        public int GetZIndex(MdiWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            if (window.View != this)
            {
                throw new ArgumentException("Window does not belong to this view.", "window");
            }

            Debug.Assert(_windows.Contains(window));
            return _windows.IndexOf(window);
        }

        /// <summary>
        ///     Adjust the window rect of a window.
        /// </summary>
        public void AdjustWindowRect(MdiWindow window, Vector delta, MdiWindowEdge interactiveEdges)
        {
            MdiPanel panel = (MdiPanel)VisualTreeHelper.GetParent(window);
            ScrollViewer contentScrollViewer = GetTemplateChild("PART_ContentScrollViewer") as ScrollViewer;

            Rect panelBounds = panel.Extents;
            Rect proposedWindowRect = MdiPanel.GetWindowRect(window);
            Rect originalProposedWindowRect = proposedWindowRect;

            if (interactiveEdges == MdiWindowEdge.None)
            {
                proposedWindowRect.X += delta.X;
                proposedWindowRect.Y += delta.Y;

                if (contentScrollViewer == null ||
                    ScrollViewer.GetHorizontalScrollBarVisibility(contentScrollViewer) == ScrollBarVisibility.Disabled)
                {
                    // Can't extend off the right.
                    proposedWindowRect.X = Math.Min(proposedWindowRect.X, panelBounds.Right - proposedWindowRect.Width);

                    // Can't extend off the left.
                    proposedWindowRect.X = Math.Max(proposedWindowRect.X, panelBounds.Left);
                }

                if (contentScrollViewer == null ||
                    ScrollViewer.GetVerticalScrollBarVisibility(contentScrollViewer) == ScrollBarVisibility.Disabled)
                {
                    // Can't extend off the bottom.
                    proposedWindowRect.Y = Math.Min(proposedWindowRect.Y, panelBounds.Bottom - proposedWindowRect.Height);

                    // Can't extend off the top.
                    proposedWindowRect.Y = Math.Max(proposedWindowRect.Y, panelBounds.Top);
                }
            }
            else
            {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Min(delta.X, (proposedWindowRect.Width - window.MinWidth));
                    if (ScrollViewer.GetHorizontalScrollBarVisibility(this) == ScrollBarVisibility.Disabled)
                    {
                        // Can't extend off the left.
                        constrainedDelta = Math.Max(constrainedDelta, panelBounds.Left - proposedWindowRect.X);
                    }

                    proposedWindowRect.X += constrainedDelta;
                    proposedWindowRect.Width -= constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Right) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Max(delta.X, -(proposedWindowRect.Width - window.MinWidth));
                    if (ScrollViewer.GetHorizontalScrollBarVisibility(this) == ScrollBarVisibility.Disabled)
                    {
                        // Can't extend off the right.
                        constrainedDelta = Math.Min(constrainedDelta, panelBounds.Right - proposedWindowRect.Right);
                    }

                    proposedWindowRect.Width += constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Min(delta.Y, (proposedWindowRect.Height - window.MinHeight));
                    if (ScrollViewer.GetVerticalScrollBarVisibility(this) == ScrollBarVisibility.Disabled)
                    {
                        // Can't extend off the top.
                        constrainedDelta = Math.Max(constrainedDelta, panelBounds.Top - proposedWindowRect.Y);
                    }

                    proposedWindowRect.Y += constrainedDelta;
                    proposedWindowRect.Height -= constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Bottom) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Max(delta.Y, -(proposedWindowRect.Height - window.MinHeight));
                    if (ScrollViewer.GetVerticalScrollBarVisibility(this) == ScrollBarVisibility.Disabled)
                    {
                        // Can't extend off the bottom.
                        constrainedDelta = Math.Min(constrainedDelta, panelBounds.Bottom - proposedWindowRect.Bottom);
                    }

                    proposedWindowRect.Height += constrainedDelta;
                }
            }

            if (EnableSnapping)
            {
                if (interactiveEdges == MdiWindowEdge.None)
                {
                    double left = proposedWindowRect.Left;
                    if (SnapXEdge(window, ref left))
                    {
                        proposedWindowRect.X = left;
                    }
                    else
                    {
                        double right = proposedWindowRect.Right;
                        if (SnapXEdge(window, ref right))
                        {
                            proposedWindowRect.X = right - proposedWindowRect.Width;
                        }
                    }

                    double top = proposedWindowRect.Top;
                    if (SnapYEdge(window, ref top))
                    {
                        proposedWindowRect.Y = top;
                    }
                    else
                    {
                        double bottom = proposedWindowRect.Bottom;
                        if (SnapYEdge(window, ref bottom))
                        {
                            proposedWindowRect.Y = bottom - proposedWindowRect.Height;
                        }
                    }
                }
                else
                {
                    // Snap the left or right edge if it is changing, but not both.
                    if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                    {
                        Debug.Assert((interactiveEdges & MdiWindowEdge.Right) == 0);
                        double left = proposedWindowRect.Left;
                        if (SnapXEdge(window, ref left))
                        {
                            proposedWindowRect.Width = proposedWindowRect.Right - left;
                            proposedWindowRect.X = left;
                        }
                    }
                    else if ((interactiveEdges & MdiWindowEdge.Right) != 0)
                    {
                        Debug.Assert((interactiveEdges & MdiWindowEdge.Left) == 0);
                        double right = proposedWindowRect.Right;
                        if (SnapXEdge(window, ref right))
                        {
                            proposedWindowRect.Width = right - proposedWindowRect.Left;
                        }
                    }

                    // Snap the top or bottom edge if it is changing, but not both.
                    if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                    {
                        Debug.Assert((interactiveEdges & MdiWindowEdge.Bottom) == 0);
                        double top = proposedWindowRect.Top;
                        if (SnapYEdge(window, ref top))
                        {
                            proposedWindowRect.Height = proposedWindowRect.Bottom - top;
                            proposedWindowRect.Y = top;
                        }
                    }
                    else if ((interactiveEdges & MdiWindowEdge.Bottom) != 0)
                    {
                        Debug.Assert((interactiveEdges & MdiWindowEdge.Top) == 0);
                        double bottom = proposedWindowRect.Bottom;
                        if (SnapYEdge(window, ref bottom))
                        {
                            proposedWindowRect.Height = bottom - proposedWindowRect.Top;
                        }
                    }
                }
            }

            if (window.MinWidth > proposedWindowRect.Width)
            {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                {
                    proposedWindowRect.X = proposedWindowRect.Right - window.MinWidth;
                }
                proposedWindowRect.Width = window.MinWidth;
            }

            if (window.MinHeight > proposedWindowRect.Height)
            {
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                {
                    proposedWindowRect.Y = proposedWindowRect.Bottom - window.MinHeight;
                }
                proposedWindowRect.Height = window.MinHeight;
            }

            MdiPanel.SetWindowRect(window, proposedWindowRect);
        }

        /// <summary>
        ///     Set the window state of a window in the view.
        /// </summary>
        /// <remarks>
        ///     MdiWindow itself will call this method when its
        ///     MdiPanel.WindowState property changes.
        /// </remarks>
        public void SetWindowState(MdiWindow window, WindowState windowState)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            if (window.View != this)
            {
                throw new ArgumentException("Window does not belong to this view.", "window");
            }

            window.SetValue(MdiPanel.WindowStateProperty, windowState);

            // The ZOrder property of the MdiWindow is coerced to be determined
            // by their orders in the window state lists.
            _windows.CoerceValues(Panel.ZIndexProperty);

            // Selection is coerced to normal/maximized windows.
            CoerceValue(Selector.SelectedItemProperty);
        }

        /// <summary>
        ///     Sets the window state of the window to maximized.
        /// </summary>
        public void MaximizeWindow(MdiWindow window)
        {
            SetWindowState(window, WindowState.Maximized);
        }

        /// <summary>
        ///     Sets the window state of the window to maximized.
        /// </summary>
        public void MinimizeWindow(MdiWindow window)
        {
            SetWindowState(window, WindowState.Minimized);
        }

        /// <summary>
        ///     Sets the window state of the window to normal.
        /// </summary>
        public void RestoreWindow(MdiWindow window)
        {
            SetWindowState(window, WindowState.Normal);
            ActivateWindow(window);
        }

        /// <summary>
        ///     Removes the window from the view.
        /// </summary>
        public void CloseWindow(MdiWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
            if (window.View != this)
            {
                throw new ArgumentException("Window does not belong to this view.", "window");
            }

            // Raise the window.Closing event, which can be canceled.
            if (window.Close())
            {
                // Removing an item may throw if the Items are bound to the ItemsSource.
                object item = ItemContainerGenerator.ItemFromContainer(window);

                // This will call back into OnContainerRemoved.
                Items.Remove(item);

                // Walk the visual tree looking for disposable elements.
                window.DisposeSubTree();
            }
        }

        /// <summary>
        ///     Activate the window.
        /// </summary>
        public void ActivateWindow(MdiWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            if (window.View != this)
            {
                throw new ArgumentException("Window does not belong to this view.", "window");
            }

            object item = ItemContainerGenerator.ItemFromContainer(window);

            if (item != SelectedItem)
            {
                SelectedItem = ItemContainerGenerator.ItemFromContainer(window);
            }
        }

        /// <summary>
        ///     Activate the previous window.
        /// </summary>
        public void ActivatePreviousWindow()
        {
            int count = Items.Count;
            int currentIndex = SelectedIndex;

            if (currentIndex >= 0)
            {
                for (int i = 1; i < count; i++)
                {
                    int candidateIndex = currentIndex - i;
                    if (candidateIndex < 0)
                    {
                        candidateIndex += count;
                    }
                    object candidateItem = Items[candidateIndex];
                    MdiWindow candidateWindow = (MdiWindow)ItemContainerGenerator.ContainerFromItem(candidateItem);

                    if (MdiPanel.GetWindowState(candidateWindow) != WindowState.Minimized)
                    {
                        ActivateWindow(candidateWindow);
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Activate the next window.
        /// </summary>
        public void ActivateNextWindow()
        {
            int count = Items.Count;
            int currentIndex = SelectedIndex;

            if (currentIndex >= 0)
            {
                for (int i = 1; i < count; i++)
                {
                    object candidateItem = Items[(currentIndex + i) % count];
                    MdiWindow candidateWindow = (MdiWindow)ItemContainerGenerator.ContainerFromItem(candidateItem);

                    if (MdiPanel.GetWindowState(candidateWindow) != WindowState.Minimized)
                    {
                        ActivateWindow(candidateWindow);
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Minimize all windows.
        /// </summary>
        public void MinimizeAllWindows()
        {
            foreach (MdiWindow window in _windows.ToArray())
            {
                SetWindowState(window, WindowState.Minimized);
            }
        }

        /// <summary>
        ///     Maximize all windows.
        /// </summary>
        public void MaximizeAllWindows()
        {
            foreach (MdiWindow window in _windows.ToArray())
            {
                SetWindowState(window, WindowState.Maximized);
            }
        }

        /// <summary>
        ///     Restore all windows.
        /// </summary>
        /// <remarks>
        ///     Minimized windows are restored behind the other windows, and
        ///     the order of maximized windows is not changed.
        /// </remarks>
        public void RestoreAllWindows()
        {
            foreach (MdiWindow window in _windows.ToArray())
            {
                SetWindowState(window, WindowState.Normal);
            }
        }

        public void FloatWindow(MdiWindow window)

        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
            if (window.View != this)
            {
                throw new ArgumentException("Window does not belong to this view.", "window");
            }

            // Get the current window rect, relative to the MdiPanel, and
            // transform it into screen coordinates.  
            HwndSource hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            MdiPanel panel = (MdiPanel)VisualTreeHelper.GetParent(window);
            Rect panelRect = MdiPanel.GetWindowRect(window);
            Rect clientRect = hwndSource.TransformDescendantToClient(panelRect, panel);
            Rect screenRect = hwndSource.TransformClientToScreen(clientRect);

            // Removing an item may throw if the Items are bound to the ItemsSource.
            object item = ItemContainerGenerator.ItemFromContainer(window);

            // This will call back into OnContainerRemoved.
            // TODO: see bug 8582
            Items.Remove(item);

            // Set these screen coordinates back into the window rect property.
            // The MdiFloater works in screen coordinates.
            MdiPanel.SetWindowRect(window, screenRect);

            MdiFloater floater = new MdiFloater();
            floater.Left = screenRect.Left;
            floater.Top = screenRect.Top;
            floater.Width = screenRect.Width;
            floater.Height = screenRect.Height;
            floater.Owner = hwndSource.RootVisual as Window;
            floater.Content = window;
            floater.Show();
        }

        /// <summary>
        ///     Tile the windows.
        /// </summary>
        public void TileWindows()
        {
            if (_windows.Count > 1)
            {
                RestoreAllWindows();

                MdiPanel panel = (MdiPanel)VisualTreeHelper.GetParent(_windows[0]);

                int cols = (int)Math.Ceiling(Math.Sqrt(_windows.Count));
                int rows = (int)Math.Ceiling((double)_windows.Count / cols);
                Point position = new Point();
                Size size = new Size(panel.RenderSize.Width / cols, panel.RenderSize.Height / rows);

                int row = 0;
                int col = 0;
                for (int i = 0; i < _windows.Count; i++)
                {
                    position.X = col * size.Width;
                    position.Y = row * size.Height;
                    MdiPanel.SetWindowRect(_windows[row * cols + col], new Rect(position, size));

                    col++;
                    if (col >= cols)
                    {
                        col = 0;
                        row++;
                    }
                }
            }
        }

        /// <summary>
        ///     Cascade the windows.
        /// </summary>
        public void CascadeWindows()
        {
            if (_windows.Count > 1)
            {
                MdiPanel panel = (MdiPanel)VisualTreeHelper.GetParent(_windows[0]);

                Point position = new Point();
                Size size = new Size(panel.RenderSize.Width / 2, panel.RenderSize.Height / 2);

                RestoreAllWindows();
                foreach (MdiWindow window in _windows)
                {
                    MdiPanel.SetWindowRect(window, new Rect(position, size));

                    position.X += 29;
                    position.Y += 29;
                }
            }
        }

        /// <summary>
        ///     Initialize the container to be in the list that its WindowState
        ///     property indicates.  Note that this is before our coercion
        ///     logic is applied.
        /// </summary>
        protected override void OnContainerAdded(MdiWindow window)
        {
            Debug.Assert(window.View == null);

            _windows.Add(window);
            _windows.BringToFront(window, MdiPanel.GetWindowState(window));

            window.View = this;

            // This virtual is called before the container is fully connected,
            // so we defer the coercion until later.
            Dispatcher.BeginInvoke((Action)(() => CoerceValue(SelectedItemProperty)));
        }

        /// <summary>
        ///     Make sure the container is removed from any list that may
        ///     contain it.
        /// </summary>
        protected override void OnContainerRemoved(MdiWindow window)
        {
            // BUG: sometimes this gets called twice for the same window.
            // try adding a plain window, then floating it
            // first time: SelectorEx.ClearContainerForItemOverride
            // second time: SelectorEx.OnItemsCollectionChanged (removed item)
            Debug.Assert(window.View == this);

            _windows.Remove(window);
            window.View = null;

            CoerceValue(SelectedItemProperty);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void ExecuteMaximizeWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Maximized);

            MaximizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void CanExecuteMaximizeWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Maximized);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void ExecuteMinimizeWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Minimized);

            MinimizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void CanExecuteMinimizeWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Minimized);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void ExecuteRestoreWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Normal);

            RestoreWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void CanExecuteRestoreWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Normal);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        private void ExecuteCloseWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            Debug.Assert(window != null && ItemsSource == null && Items != null);

            CloseWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        /// <remarks>
        ///     The command is only enabled if the items are not bound via
        ///     the ItemsSource.
        /// </remarks>
        private void CanExecuteCloseWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && ItemsSource == null && Items != null);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivateWindow command.
        /// </summary>
        private void ExecuteActivateWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            ActivateWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.ActivateWindow command.
        /// </summary>
        private void CanExecuteActivateWindow(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivateNextWindow command.
        /// </summary>
        private void ExecuteActivateNextWindow(ExecutedRoutedEventArgs e)
        {
            ActivateNextWindow();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivatePreviousWindow command.
        /// </summary>
        private void ExecuteActivatePreviousWindow(ExecutedRoutedEventArgs e)
        {
            ActivatePreviousWindow();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void ExecuteAdjustWindowRect(ExecutedRoutedEventArgs e)
        {
            UIElement originalSource = (UIElement)e.OriginalSource;
            AdjustWindowRectParameter swp = (AdjustWindowRectParameter)e.Parameter;

            MdiWindow window = ContainerFromElement(originalSource);
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);

            MdiPanel panel = (MdiPanel)VisualTreeHelper.GetParent(window);
            Vector delta = originalSource.TransformElementToElement(swp.Delta, panel);

            AdjustWindowRect(window, delta, swp.InteractiveEdges);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void CanExecuteAdjustWindowRect(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CascadeWindows command.
        /// </summary>
        private void ExecuteCascadeWindows(ExecutedRoutedEventArgs e)
        {
            Debug.Assert(_windows.Count > 1);
            CascadeWindows();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.TileWindows command.
        /// </summary>
        private void ExecuteTileWindows(ExecutedRoutedEventArgs e)
        {
            Debug.Assert(_windows.Count > 1);
            TileWindows();
        }

        /// <summary>
        ///     CanExecute handler for any command that requires more than one window.
        /// </summary>
        private void CanExecuteCommandThatRequiresMoreThanOneWindow(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _windows.Count > 1;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeAllWindows command.
        /// </summary>
        private void ExecuteMinimizeAllWindows(ExecutedRoutedEventArgs e)
        {
            Debug.Assert(_windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Minimized).Count() > 0);
            MinimizeAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeAllWindows command.
        /// </summary>
        private void CanExecuteMinimizeAllWindows(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Minimized).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeAllWindows command.
        /// </summary>
        private void ExecuteMaximizeAllWindows(ExecutedRoutedEventArgs e)
        {
            Debug.Assert(_windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Maximized).Count() > 0);
            MaximizeAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeAllWindows command.
        /// </summary>
        private void CanExecuteMaximizeAllWindows(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Maximized).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreAllWindows command.
        /// </summary>
        private void ExecuteRestoreAllWindows(ExecutedRoutedEventArgs e)
        {
            Debug.Assert(_windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Normal).Count() > 0);
            RestoreAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreAllWindows command.
        /// </summary>
        private void CanExecuteRestoreAllWindows(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Normal).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.FloatWindow command.
        /// </summary>
        private void ExecuteFloatWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);

            FloatWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.FloatWindow command.
        /// </summary>
        private void CanExecuteFloatWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = ContainerFromElement((DependencyObject)e.OriginalSource);
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);
        }

        private bool SnapYEdge(MdiWindow window, ref double y)
        {
            foreach (MdiWindow otherWindow in _windows)
            {
                // Skip the window itself.
                if (otherWindow == window) { continue; }

                // Skip any child that is not in the normal state.
                WindowState windowState = MdiPanel.GetWindowState(otherWindow);
                if (windowState != WindowState.Normal) { continue; }

                Rect windowRect = MdiPanel.GetWindowRect(otherWindow);
                if (Math.Abs(windowRect.Top - y) < SnapThreshold)
                {
                    // Snap to the top edge of the child.
                    y = windowRect.Top;
                    return true;
                }
                else if (Math.Abs(windowRect.Top + otherWindow.RenderSize.Height - y) < SnapThreshold)
                {
                    // Snap to the bottom edge of the child.
                    y = windowRect.Top + otherWindow.RenderSize.Height;
                    return true;
                }
            }

            return false;
        }

        private bool SnapXEdge(MdiWindow window, ref double x)
        {
            foreach (MdiWindow otherWindow in _windows)
            {
                // Skip the window itself.
                if (otherWindow == window) { continue; }

                // Skip any child that is not in the normal state.
                WindowState windowState = MdiPanel.GetWindowState(otherWindow);
                if (windowState != WindowState.Normal) { continue; }

                Rect windowRect = MdiPanel.GetWindowRect(otherWindow);
                if (Math.Abs(windowRect.Left - x) < SnapThreshold)
                {
                    // Snap to the left edge of the child.
                    x = windowRect.Left;
                    return true;
                }
                else if (Math.Abs(windowRect.Left + otherWindow.RenderSize.Width - x) < SnapThreshold)
                {
                    // Snap to the right edge of the child.
                    x = windowRect.Left + otherWindow.RenderSize.Width;
                    return true;
                }
            }

            return false;
        }

        private object OnCoerceSelectedItem(object baseValue)
        {
            object selectedItem = baseValue;
            MdiWindow selectedWindow = null;
            if (selectedItem != null)
            {
                selectedWindow = (MdiWindow)ItemContainerGenerator.ContainerFromItem(selectedItem);
                if (selectedWindow != null && MdiPanel.GetWindowState(selectedWindow) == WindowState.Minimized)
                {
                    selectedWindow = null;
                }
            }

            // If selection is not valid, or null, try to select a non-minimized window.
            if (selectedWindow == null)
            {
                selectedWindow = _windows.Where((w) => MdiPanel.GetWindowState(w) != WindowState.Minimized).LastOrDefault();
            }

            if (selectedWindow != null)
            {
                // Map the newly activated window to the underlying item to select.
                selectedItem = ItemContainerGenerator.ItemFromContainer(selectedWindow);
            }
            else
            {
                selectedItem = null;
            }

            return selectedItem;
        }

        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            object selection = e.NewValue;
            if (selection != null)
            {
                MdiWindow window = (MdiWindow)ItemContainerGenerator.ContainerFromItem(e.NewValue);

                // Bring the newly activated window to the front.
                _windows.BringToFront(window, MdiPanel.GetWindowState(window));
                _windows.CoerceValues(Panel.ZIndexProperty);

                // Give the newly activated window focus.
                window.Focus();
            }
        }

        MdiWindowCollection _windows = new MdiWindowCollection();
    }
}
