using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace CustomTheme
{
    public class MahAppsPaletteHelper : PaletteHelper
    {
        public override void ReplacePalette(Palette palette)
        {
            base.ReplacePalette(palette);

            List<Hue> primaryColors = palette.PrimarySwatch.PrimaryHues.ToList();

            Hue dark = primaryColors[palette.PrimaryDarkHueIndex];
            Hue mid = primaryColors[palette.PrimaryMidHueIndex];
            Hue light = primaryColors[palette.PrimaryLightHueIndex];

            //Replace MahApps brushes
            //This replacement should mirror what is done in the App.xaml MahApps Brushes
            ReplaceEntry("HighlightBrush", new SolidColorBrush(dark.Color));
            ReplaceEntry("AccentBaseColorBrush", new SolidColorBrush(mid.Color));
            ReplaceEntry("AccentColorBrush", new SolidColorBrush(mid.Color));
            ReplaceEntry("AccentColorBrush2", new SolidColorBrush(mid.Color) { Opacity = 0.8 });
            ReplaceEntry("AccentColorBrush3", new SolidColorBrush(light.Color));
            ReplaceEntry("AccentColorBrush4", new SolidColorBrush(light.Color) { Opacity = 0.8 });

            ReplaceEntry("WindowTitleColorBrush", new SolidColorBrush(dark.Color));

            ReplaceEntry("ProgressBrush", new LinearGradientBrush(dark.Color, mid.Color, 90));

            ReplaceEntry("CheckmarkFill", new SolidColorBrush(mid.Color));
            ReplaceEntry("RightArrowFill", new SolidColorBrush(mid.Color));

            ReplaceEntry("IdealForegroundColorBrush", new SolidColorBrush(mid.Foreground));
            ReplaceEntry("IdealForegroundDisabledBrush", new SolidColorBrush(mid.Foreground) { Opacity = 0.4 });
            ReplaceEntry("AccentSelectedColorBrush", new SolidColorBrush(mid.Foreground));

            ReplaceEntry("MetroDataGrid.HighlightBrush", new SolidColorBrush(mid.Color));
            ReplaceEntry("MetroDataGrid.HighlightTextBrush", new SolidColorBrush(mid.Foreground));
            ReplaceEntry("MetroDataGrid.MouseOverHighlightBrush", new SolidColorBrush(light.Foreground));
            ReplaceEntry("MetroDataGrid.FocusBorderBrush", new SolidColorBrush(mid.Color));
            ReplaceEntry("MetroDataGrid.InactiveSelectionHighlightBrush", new SolidColorBrush(mid.Color));
            ReplaceEntry("MetroDataGrid.InactiveSelectionHighlightTextBrush", new SolidColorBrush(mid.Foreground));

            ReplaceEntry("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10", new SolidColorBrush(mid.Color));
            ReplaceEntry("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10", new SolidColorBrush(mid.Color) { Opacity = 0.8 });
            ReplaceEntry("MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10", new SolidColorBrush(mid.Foreground));
        }

        //This is pretty much a copy of what is in MDIX since it is private
        private static void ReplaceEntry(object entryName, object newValue, ResourceDictionary parentDictionary = null)
        {
            if (parentDictionary == null)
                parentDictionary = Application.Current.Resources;

            if (parentDictionary.Contains(entryName))
            {
                if (parentDictionary[entryName] is SolidColorBrush brush && !brush.IsFrozen)
                {
                    var animation = new ColorAnimation
                    {
                        From = ((SolidColorBrush)parentDictionary[entryName]).Color,
                        To = ((SolidColorBrush)newValue).Color,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    parentDictionary[entryName] = newValue; //Set value normally
                }
            }

            foreach (var dictionary in parentDictionary.MergedDictionaries)
                ReplaceEntry(entryName, newValue, dictionary);
        }
    }
}