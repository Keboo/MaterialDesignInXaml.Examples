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

namespace Microsoft.DwayneNeed.Controls
{
    public class ChildWindow : Control
    {
        static ChildWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChildWindow), new FrameworkPropertyMetadata(typeof(ChildWindow)));
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(ChildWindow),
            new FrameworkPropertyMetadata(null));

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

    }
}
