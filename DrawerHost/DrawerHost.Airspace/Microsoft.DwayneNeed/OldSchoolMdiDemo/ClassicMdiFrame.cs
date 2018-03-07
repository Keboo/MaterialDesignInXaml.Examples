using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Windows.Interop;
using Microsoft.DwayneNeed.Extensions;
using System.Runtime.InteropServices;

namespace OldSchoolMdiDemo
{
    // 
    public class ClassicMdiFrame : Window
    {
        static ClassicMdiFrame()
        {
            EventManager.RegisterClassHandler(
                typeof(ClassicMdiFrame),
                NotifyMdiClientEvent,
                (RoutedEventHandler)OnNotifyMdiClientEvent);
        }

        public ClassicMdiFrame()
        {
            _hook = new HwndSourceHook(OnMessage);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PresentationSource.AddSourceChangedHandler(this, OnPresentationSourceChanged);
            }
        }

        public static RoutedEvent NotifyMdiClientEvent = EventManager.RegisterRoutedEvent(
            "NotifyMdiClient",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ClassicMdiFrame));

        private void OnPresentationSourceChanged(object sender, SourceChangedEventArgs e)
        {
            HwndSource oldHwndSource = e.OldSource as HwndSource;
            if(oldHwndSource != null)
            {
                oldHwndSource.RemoveHook(_hook);
            }

            HwndSource newHwndSource = e.NewSource as HwndSource;
            if(newHwndSource != null)
            {
                newHwndSource.AddHook(_hook);
            }
        }

        // A proper MDI frame window proc must adhere to certain rules, which
        // include calling DefFrameProc instead of DefWindowProc.  Since the
        // default behavior for WPF is to call DefWindowProc, we have to hook
        // all messages and handle them, calling DefFrameProc ourselves.
        private IntPtr OnMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = true;
            IntPtr hwndMdiClient = _mdiClient != null ? _mdiClient.Handle : IntPtr.Zero;
            return DefFrameProc(hwnd, hwndMdiClient, msg, wParam, lParam);
        }

        // Somewhere in the visual tree should be the MDI client window.  It
        // will notify us, and we will remember it.
        private static void OnNotifyMdiClientEvent(object sender, RoutedEventArgs args)
        {
            ClassicMdiFrame _this = (ClassicMdiFrame) sender;
            _this._mdiClient = (ClassicMdiClient) args.OriginalSource;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr DefFrameProc(IntPtr hwnd, IntPtr hwndMDIClient, int msg, IntPtr wParam, IntPtr lParam);

        private HwndSourceHook _hook;
        private ClassicMdiClient _mdiClient;
    }
}
