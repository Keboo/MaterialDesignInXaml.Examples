using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Interop;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

namespace Microsoft.DwayneNeed.Controls
{
    [ContentProperty("Child")]
    public class OldSchoolMdiWindow : FrameworkElement
    {
        public OldSchoolMdiWindow()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PresentationSource.AddSourceChangedHandler(this, OnPresentationSourceChanged);
            }
        }

        public FrameworkElement Child
        {
            get
            {
                return _child;
            }
            set
            {
                if (_child != null)
                {
                    if (_hwndSourceChild != null)
                    {
                        _hwndSourceChild.RootVisual = null;
                    }

                    RemoveLogicalChild(_child);
                }

                _child = value;

                if (_child != null)
                {
                    AddLogicalChild(_child);

                    if (_hwndSourceChild != null)
                    {
                        _hwndSourceChild.RootVisual = _child;
                    }
                }
            }
        }

        /// <summary> 
        /// Returns enumerator to logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_child != null)
                {
                    yield return _child;
                }
            }
        }

        // This method handles the situation where the PresentationSource that
        // contains this OldSchoolMdiChild instance changes.  This will happen
        // the first time, and possibly if someone programatically removes it
        // and inserts it somewhere else.
        private void OnPresentationSourceChanged(object sender, SourceChangedEventArgs e)
        {
            // Destroy our PresentationSource children when we are disconnected.
            if (e.OldSource != null)
            {
                if (_hwndSourceChild != null && !_hwndSourceChild.IsDisposed)
                {
                    _hwndSourceChild.RootVisual = null;
                    _hwndSourceChild.Dispose();
                    _hwndSourceChild = null;
                }
            }

            // Create our PresentationSource children when we are connected.
            HwndSource newSource = e.NewSource as HwndSource;
            if (newSource != null)
            {
                var p = new HwndSourceParameters("MiddleChild", 200, 100);
                p.WindowClassStyle = 0;
                p.WindowStyle = WS_OVERLAPPEDWINDOW | WS_CHILD | WS_CLIPCHILDREN | WS_CLIPSIBLINGS;
                p.ExtendedWindowStyle = 0;
                p.ParentWindow = newSource.Handle;
                p.HwndSourceHook = ChildHook;
                _hwndSourceChild = new HwndSource(p);
                _hwndSourceChild.RootVisual = Child;

                // Show the window for the first time normally.
                ShowWindow(_hwndSourceChild.Handle, SW_SHOWNORMAL); // First time
                SetWindowPos(_hwndSourceChild.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }
        }

        private IntPtr ChildHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_WINDOWPOSCHANGING)
            {
                unsafe
                {
                    WINDOWPOS* pWindowPos = (WINDOWPOS*)lParam;
                    pWindowPos->flags |= SWP_NOCOPYBITS;
                }
            }
            else if (msg == WM_CHILDACTIVATE || msg == WM_MOUSEACTIVATE)
            {
                SetWindowPos(hwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }
            else if (msg == WM_ERASEBKGND)
            {
                handled = true;
                return new IntPtr(1);
            }

            return IntPtr.Zero;
        }

        private const int WS_OVERLAPPED = 0x00000000;
        private const int WS_POPUP = unchecked((int)0x80000000);
        private const int WS_CHILD = 0x40000000;
        private const int WS_MINIMIZE = 0x20000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_DISABLED = 0x08000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;
        private const int WS_CLIPCHILDREN = 0x02000000;
        private const int WS_MAXIMIZE = 0x01000000;
        private const int WS_CAPTION = 0x00C00000;     // WS_BORDER | WS_DLGFRAME
        private const int WS_BORDER = 0x00800000;
        private const int WS_DLGFRAME = 0x00400000;
        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;
        private const int WS_SYSMENU = 0x00080000;
        private const int WS_THICKFRAME = 0x00040000;
        private const int WS_GROUP = 0x00020000;
        private const int WS_TABSTOP = 0x00010000;

        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_MAXIMIZEBOX = 0x00010000;

        private const int WS_TILED = WS_OVERLAPPED;
        private const int WS_ICONIC = WS_MINIMIZE;
        private const int WS_SIZEBOX = WS_THICKFRAME;
        private const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

        private const int WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        private const int WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;
        private const int WS_CHILDWINDOW = WS_CHILD;

        private const int WM_SIZE = 0x0005;
        private const int WM_WINDOWPOSCHANGED = 0x0047;
        private const int WM_WINDOWPOSCHANGING = 0x0046;
        private const int WM_CHILDACTIVATE = 0x0022;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int WM_ERASEBKGND = 0x0014;

        private readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private readonly IntPtr HWND_TOP = new IntPtr(0);
        private readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        private const int SW_FORCEMINIMIZE = 11;
        private const int SW_HIDE = 0;
        private const int SW_MAXIMIZE=3;
        private const int SW_MINIMIZE=6;
        private const int SW_RESTORE=9;
        private const int SW_SHOW=5;
        private const int SW_SHOWDEFAULT=10;
        private const int SW_SHOWMAXIMIZED=3;
        private const int SW_SHOWMINIMIZED=2;
        private const int SW_SHOWMINNOACTIVE=7;
        private const int SW_SHOWNA=8;
        private const int SW_SHOWNOACTIVATE=4;
        private const int SW_SHOWNORMAL=1;

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOREDRAW = 0x0008;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_FRAMECHANGED = 0x0020; // The frame changed: send WM_NCCALCSIZE
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SWP_HIDEWINDOW = 0x0080;
        private const int SWP_NOCOPYBITS = 0x0100;
        private const int SWP_NOOWNERZORDER = 0x0200;  // Don't do owner Z ordering
        private const int SWP_NOSENDCHANGING = 0x0400;  // Don't send WM_WINDOWPOSCHANGING
        private const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        private const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        private const int SWP_DEFERERASE = 0x2000;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;

        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hwnd, int index);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hwnd, int index);

        private static IntPtr GetWindowLongPtr(IntPtr hwnd, int index)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLongPtr32(hwnd, index);
            }
            else
            {
                return GetWindowLongPtr64(hwnd, index);
            }
        }

        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;

        private HwndSource _hwndSourceChild;
        private FrameworkElement _child;
    }
}
