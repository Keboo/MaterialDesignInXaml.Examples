using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using Microsoft.DwayneNeed.Interop;
using Microsoft.DwayneNeed.Win32.User32;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for D2DSample.xaml
    /// </summary>
    public partial class D2DSample : Grid
    {
        public D2DSample()
        {
            InitializeComponent();

            // TODO: DestroyWindow
            CallbackHwndHost hwndHost = new CallbackHwndHost((parent) => CreateDemoHelloWorldApp(parent), (hwnd) => { });

            TheGrid.Children.Add(hwndHost);
        }

        [DllImport("D2DSamples.dll")]
        private static extern HWND CreateDemoHelloWorldApp(HWND parent);
    }
}
