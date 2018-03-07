using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Collections;
using Microsoft.DwayneNeed.Threading;
using System.Diagnostics;
using Microsoft.DwayneNeed.Interop;
using System.Windows.Threading;

namespace Microsoft.DwayneNeed.Controls
{
    [UsableDuringInitialization(false)]
    public class HwndSourceElement : HwndHostEx
    {
        public HwndSourceParameters? CreationParameters {get;set;}

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(HwndSourceElement),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnChildSourceChanged)));

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            // Let our base class create the standard window.  We will simply
            // use it as the parent of our HwndSource.
            // HandleRef hwndChild = base.BuildWindowCore(hwndParent);

            HwndSourceParameters? optCreationParameters = CreationParameters;

            if (_thread != null)
            {
                // Asynchronously create the HwndSource on the worker thread.
                _thread.Dispatcher.BeginInvoke((Action)delegate
                {
                    _hwndSource = CreateHwndSource(optCreationParameters, hwndParent.Handle);
                });
            }
            else
            {
                // Synchronously create the HwndSource on the UI thread.
                _hwndSource = CreateHwndSource(optCreationParameters, hwndParent.Handle);
            }

            UpdateRootVisual(Source);

            return new HandleRef(this,_hwndSource.Handle);
        }


        /// <summary> 
        /// Returns enumerator to logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                // Only consider the root of our child HwndSource a logical
                // child if it is on the same thread.
                if(_thread == null && _hwndSource == null)
                {
                    yield return (FrameworkElement) _hwndSource.RootVisual;
                }
            }
        }

        private static HwndSource CreateHwndSource(HwndSourceParameters? optCreationParameters, IntPtr hwndParent)
        {
            HwndSourceParameters creationParameters = new HwndSourceParameters();

            if (optCreationParameters.HasValue)
            {
                creationParameters = optCreationParameters.Value;
            }

            // In all cases, the HwndSource parameters are adjusted so that:
            // 1) it is forced to be a child window
            // 2) it is forced to have our child window as its parent
            // 3) it is forced to clip children and siblings
            creationParameters.WindowStyle |= WS_CHILD | WS_CLIPSIBLINGS | WS_CLIPCHILDREN | WS_VISIBLE;
            creationParameters.ParentWindow = hwndParent;

            HwndSource hwndSource = new HwndSource(creationParameters);

            // The size of the HwndSource window is enforced to be the
            // size of the parent window.
            hwndSource.SizeToContent = SizeToContent.Manual;
            //hwndSource.AddHook(new HwndSourceHook(HwndSourceWndProcHook));

            return hwndSource;
        }

        private static void OnChildSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HwndSourceElement that = (HwndSourceElement)d;

            Uri newChildSource = (Uri)e.NewValue;
            HwndSource hwndSource = that._hwndSource;

            if (hwndSource != null)
            {
                if (that._thread != null)
                {
                    // Asynchronously load the Uri on the worker thread.
                    that._thread.Dispatcher.BeginInvoke((Action)delegate
                    {
                        hwndSource.RootVisual = (FrameworkElement)Application.LoadComponent(newChildSource);
                    });
                }
                else
                {
                    // Synchronously load the Uri on the UI thread.
                    that.UpdateRootVisual(newChildSource);
                }
            }
        }

        private void UpdateRootVisual(Uri source)
        {
            FrameworkElement oldRoot = (FrameworkElement) _hwndSource.RootVisual;
            if(oldRoot != null)
            {
                RemoveLogicalChild(oldRoot);
            }

            if (source != null)
            {
                FrameworkElement newRoot = (FrameworkElement)Application.LoadComponent(source);
                if (newRoot != null)
                {
                    _hwndSource.RootVisual = newRoot;
                    AddLogicalChild(newRoot);
                }
            }
        }

        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;
        private const int WS_CLIPCHILDREN = 0x02000000;
        private const int WS_VISIBLE = 0x10000000;

        private const int WM_WINDOWPOSCHANGING = 0x0046;
        private const int WM_WINDOWPOSCHANGED = 0x0047;

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

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private const int SWP_NOCOPYBITS = 0x0100;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

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

        [DllImport("user32.dll")]
        private static extern IntPtr CreateWindowEx(
            int exStyle,
            string className,
            string windowName,
            int style,
            int x,
            int y,
            int width,
            int height,
            IntPtr parent,
            IntPtr menu,
            IntPtr module,
            IntPtr createParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr after, int X, int Y, int cx, int cy, int flags);


        private HwndSource _hwndSource;
        //private HwndHook _hwndSourceHook;
        private UIThreadPoolThread _thread;
    }
}
