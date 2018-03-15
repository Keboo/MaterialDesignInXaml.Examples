using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.DwayneNeed.Extensions;
using System.Diagnostics;

namespace Microsoft.DwayneNeed.MDI
{
    public class MdiFloater : Window
    {
        static MdiFloater()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiFloater), new FrameworkPropertyMetadata(typeof(MdiFloater)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.AdjustWindowRect,
                /*     Execute:     */ (s, e) => ((MdiFloater)s).ExecuteAdjustWindowRect(e),
                /*     CanExecute:  */ (s, e) => ((MdiFloater)s).CanExecuteAdjustWindowRect(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MaximizeWindow,
                /*     Execute:     */ (s, e) => ((MdiFloater)s).ExecuteMaximizeWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiFloater)s).CanExecuteMaximizeWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.MinimizeWindow,
                /*     Execute:     */ (s, e) => ((MdiFloater)s).ExecuteMinimizeWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiFloater)s).CanExecuteMinimizeWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.RestoreWindow,
                /*     Execute:     */ (s, e) => ((MdiFloater)s).ExecuteRestoreWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiFloater)s).CanExecuteRestoreWindow(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ MdiCommands.CloseWindow,
                /*     Execute:     */ (s, e) => ((MdiFloater)s).ExecuteCloseWindow(e),
                /*     CanExecute:  */ (s, e) => ((MdiFloater)s).CanExecuteCloseWindow(e)));

        }

        /// <summary>
        ///     Adjust the window rect of a window.
        /// </summary>
        public void AdjustWindowRect(MdiWindow window, Vector delta, MdiWindowEdge interactiveEdges)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            if (Content != window)
            {
                throw new ArgumentException("Window does not belong to this floater.", "window");
            }

            Rect screenRect = MdiPanel.GetWindowRect(window);

            if (interactiveEdges == MdiWindowEdge.None)
            {
                screenRect.X += delta.X;
                screenRect.Y += delta.Y;
            }
            else
            {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Min(delta.X, (screenRect.Width - window.MinWidth));

                    screenRect.X += constrainedDelta;
                    screenRect.Width -= constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Right) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Max(delta.X, -(screenRect.Width - window.MinWidth));

                    screenRect.Width += constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Min(delta.Y, (screenRect.Height - window.MinHeight));

                    screenRect.Y += constrainedDelta;
                    screenRect.Height -= constrainedDelta;
                }
                if ((interactiveEdges & MdiWindowEdge.Bottom) != 0)
                {
                    // Can't size smaller than the minimum size.
                    double constrainedDelta = Math.Max(delta.Y, -(screenRect.Height - window.MinHeight));

                    screenRect.Height += constrainedDelta;
                }
            }

            if (window.MinWidth > screenRect.Width)
            {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                {
                    screenRect.X = screenRect.Right - window.MinWidth;
                }
                screenRect.Width = window.MinWidth;
            }

            if (window.MinHeight > screenRect.Height)
            {
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                {
                    screenRect.Y = screenRect.Bottom - window.MinHeight;
                }
                screenRect.Height = window.MinHeight;
            }

            MdiPanel.SetWindowRect(window, screenRect);

            this.Left = screenRect.Left;
            this.Top = screenRect.Top;
            this.Width = screenRect.Width;
            this.Height = screenRect.Height;
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

            if (Content != window)
            {
                throw new ArgumentException("Window does not belong to this floater.", "window");
            }

            window.SetValue(MdiPanel.WindowStateProperty, windowState);
            this.WindowState = windowState;
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

            if (Content != window)
            {
                throw new ArgumentException("Window does not belong to this floater.", "window");
            }

            // Raise the window.Closing event, which can be canceled.
            if (window.Close())
            {
                // close this floater.
                Close();

                // Walk the visual tree looking for disposable elements.
                window.DisposeSubTree();
            }
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void ExecuteAdjustWindowRect(ExecutedRoutedEventArgs e)
        {
            UIElement originalSource = (UIElement)e.OriginalSource;
            AdjustWindowRectParameter swp = (AdjustWindowRectParameter)e.Parameter;

            MdiWindow window = Content as MdiWindow;
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);

            Vector delta = originalSource.TransformElementToElement(swp.Delta, window);

            AdjustWindowRect(window, delta, swp.InteractiveEdges);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void CanExecuteAdjustWindowRect(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) == WindowState.Normal);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void ExecuteMaximizeWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Maximized);

            MaximizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void CanExecuteMaximizeWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Maximized);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void ExecuteMinimizeWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Minimized);

            MinimizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void CanExecuteMinimizeWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Minimized);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void ExecuteRestoreWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            Debug.Assert(window != null && MdiPanel.GetWindowState(window) != WindowState.Normal);

            RestoreWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void CanExecuteRestoreWindow(CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            e.CanExecute = (window != null && MdiPanel.GetWindowState(window) != WindowState.Normal);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        private void ExecuteCloseWindow(ExecutedRoutedEventArgs e)
        {
            MdiWindow window = Content as MdiWindow;
            Debug.Assert(window != null);

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
            MdiWindow window = Content as MdiWindow;
            e.CanExecute = (window != null);
        }
    }
}
