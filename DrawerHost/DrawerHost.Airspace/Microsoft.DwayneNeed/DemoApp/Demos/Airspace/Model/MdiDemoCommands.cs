using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DemoApp.Demos.Airspace.Model
{
    public static class MdiDemoCommands
    {
        public static RoutedCommand NewWorkspace = new RoutedCommand("NewWorkspace", typeof(MdiDemoCommands));
        public static RoutedCommand OpenWorkspace = new RoutedCommand("OpenWorkspace", typeof(MdiDemoCommands));
        public static RoutedCommand SaveWorkspace = new RoutedCommand("SaveWorkspace", typeof(MdiDemoCommands));
        public static RoutedCommand SaveWorkspaceAs = new RoutedCommand("SaveWorkspaceAs", typeof(MdiDemoCommands));
    }
}
