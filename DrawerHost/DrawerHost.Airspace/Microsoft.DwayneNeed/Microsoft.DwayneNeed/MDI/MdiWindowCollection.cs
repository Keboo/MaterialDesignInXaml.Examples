using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.DwayneNeed.Interop;
using Microsoft.DwayneNeed.Win32.User32;
using Microsoft.DwayneNeed.Win32;

namespace Microsoft.DwayneNeed.MDI
{
    internal class MdiWindowCollection : ObservableCollection<MdiWindow>
    {
        // Bring to the front of the windows in the specified state.
        public void BringToFront(MdiWindow window, WindowState windowState)
        {
            int oldIndex = IndexOf(window);
            int newIndex = Count - 1;

            if (windowState == WindowState.Minimized)
            {
                bool foundSelf = false;

                for (newIndex = 0; newIndex < (Count - 1); newIndex++)
                {
                    if (Items[newIndex] == window)
                    {
                        foundSelf = true;

                    }
                    if (MdiPanel.GetWindowState(Items[newIndex]) != WindowState.Minimized)
                    {
                        break;
                    }
                }

                if (foundSelf)
                {
                    newIndex--;
                }
            }

            Move(oldIndex, newIndex);

            // HACK: how to coordinate Win32 ZOrder with WPF ZOrder?  This works, but assumes to many implementation details.
            if (VisualTreeHelper.GetChildrenCount(window) > 0)
            {
                AirspaceDecorator hwndClipper = VisualTreeHelper.GetChild(window, 0) as AirspaceDecorator;
                if (hwndClipper != null)
                {
                    if (VisualTreeHelper.GetChildrenCount(hwndClipper) > 0)
                    {
                        HwndSourceHost hwndSourceHost = VisualTreeHelper.GetChild(hwndClipper, 0) as HwndSourceHost;
                        if (hwndSourceHost != null)
                        {
                            HWND hwnd = new HWND(hwndSourceHost.Handle);
                            NativeMethods.SetWindowPos(hwnd, HWND.TOP, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE);
                        }
                    }
                }
            }
        }

        public void CoerceValues(DependencyProperty dp)
        {
            foreach (MdiWindow window in Items)
            {
                window.CoerceValue(dp);
            }
        }
    }
}
