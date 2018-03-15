using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.DwayneNeed.Input;

namespace Microsoft.DwayneNeed.MDI
{
    public static class MdiCommands
    {
        public static RoutedCommand<AdjustWindowRectParameter> AdjustWindowRect = new RoutedCommand<AdjustWindowRectParameter>("SizeWindow", typeof(MdiCommands));
        public static RoutedCommand RestoreWindow = new RoutedCommand("RestoreWindow", typeof(MdiCommands));
        public static RoutedCommand MaximizeWindow = new RoutedCommand("MaximizeWindow", typeof(MdiCommands));
        public static RoutedCommand MinimizeWindow = new RoutedCommand("MinimizeWindow", typeof(MdiCommands));
        public static RoutedCommand CloseWindow = new RoutedCommand("CloseWindow", typeof(MdiCommands));
        public static RoutedCommand ActivateWindow = new RoutedCommand("ActivateWindow", typeof(MdiCommands));
        public static RoutedCommand ActivateNextWindow = new RoutedCommand("ActivateNextWindow", typeof(MdiCommands));
        public static RoutedCommand ActivatePreviousWindow = new RoutedCommand("ActivatePreviousWindow", typeof(MdiCommands));
        public static RoutedCommand CascadeWindows = new RoutedCommand("CascadeWindows", typeof(MdiCommands));
        public static RoutedCommand TileWindows = new RoutedCommand("TileWindows", typeof(MdiCommands));
        public static RoutedCommand MinimizeAllWindows = new RoutedCommand("MinimizeAllWindows", typeof(MdiCommands));
        public static RoutedCommand MaximizeAllWindows = new RoutedCommand("MaximizeAllWindows", typeof(MdiCommands));
        public static RoutedCommand RestoreAllWindows = new RoutedCommand("RestoreAllWindows", typeof(MdiCommands));
        public static RoutedCommand FloatWindow = new RoutedCommand("FloatWindow", typeof(MdiCommands));
    }
}
