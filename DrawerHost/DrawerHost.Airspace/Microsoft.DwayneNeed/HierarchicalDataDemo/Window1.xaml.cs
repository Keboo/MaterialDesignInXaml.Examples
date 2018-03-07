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
using System.Diagnostics;

namespace HierarchicalDataDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HeiankyoView h = new HeiankyoView();
            Rect rc1 = h.Add(new Size(4, 2));
            Rect rc2 = h.Add(new Size(2, 4));
            Rect rc3 = h.Add(new Size(2, 2));
            Debug.WriteLine(rc1);
            Debug.WriteLine("done");
        }
    }
}
