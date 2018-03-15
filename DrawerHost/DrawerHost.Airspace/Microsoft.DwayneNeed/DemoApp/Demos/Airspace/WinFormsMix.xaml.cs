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
using System.Windows.Forms.Integration;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for WinFormsMix.xaml
    /// </summary>
    public partial class WinFormsMix : WindowsFormsHost
    {
        public WinFormsMix()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            PropertyMap.Remove("Background");
            EnableWindowsFormsInterop();

            InitializeComponent();
        }
    }
}
