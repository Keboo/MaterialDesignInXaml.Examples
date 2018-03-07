using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace OldSchoolMdiDemo
{
    // Represents the MDI Client window that must be located somewhere within
    // the visual tree of the ClassicMdiWindow.
    public class ClassicMdiClient : HwndHost
    {
        public ClassicMdiClient()
        {
            Loaded += new RoutedEventHandler(OnLoaded);
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs notifyMdiClientEventArgs = new RoutedEventArgs(ClassicMdiFrame.NotifyMdiClientEvent, this);
            RaiseEvent(notifyMdiClientEventArgs);
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            CLIENTCREATESTRUCT ccs = new CLIENTCREATESTRUCT();
            ccs.hWindowMenu = IntPtr.Zero; // Hopefully this is not needed!
            ccs.idFirstChild = 0;

            IntPtr hwnd = CreateWindowEx(
            ExStyle:0,
            ClassName:"MDICLIENT",
            WindowName:"ClassicMdiClient",
            Style: WS_CHILD | WS_CLIPSIBLINGS | WS_CLIPCHILDREN,
            X:0,
            Y:0,
            Width:0,
            Height:0,
            Parent:hwndParent,
            Menu:IntPtr.Zero,
            ModuleInstance:IntPtr.Zero,
            CreateParam:ref ccs);

            return new HandleRef(this, hwnd);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd);
        }


        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;
        private const int WS_CLIPCHILDREN = 0x02000000;

        [StructLayout(LayoutKind.Sequential)]
        private struct CLIENTCREATESTRUCT
        {
            public IntPtr hWindowMenu;
            public int idFirstChild;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowEx(
           int ExStyle,
           string ClassName,
           string WindowName,
           int Style,
           int X,
           int Y,
           int Width,
           int Height,
           HandleRef Parent,
           IntPtr Menu,
           IntPtr ModuleInstance,
           ref CLIENTCREATESTRUCT CreateParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyWindow(HandleRef hwnd);
     }
}
