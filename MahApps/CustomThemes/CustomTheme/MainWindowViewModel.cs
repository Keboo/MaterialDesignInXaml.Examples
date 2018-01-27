using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomTheme
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MahAppsPaletteHelper _PaletteHelper = new MahAppsPaletteHelper();

        public List<ThemeColorViewModel> ThemeColors { get; } = new List<ThemeColorViewModel>();

        public ICommand SetThemeCommand { get; }

        //The starting value of this must match which resource dictionary we loaded in the App.xaml, in this case the light MDIX theme
        private bool _IsLightTheme = true;
        public bool IsLightTheme
        {
            get => _IsLightTheme;
            set
            {
                if (Set(ref _IsLightTheme, value))
                {
                    _PaletteHelper.SetLightDark(!value);
                }
            }
        }

        public MainWindowViewModel()
        {
            var swatchProvider = new SwatchesProvider();
            ThemeColors.AddRange(swatchProvider.Swatches
                .Where(x => x.IsAccented) //This sample assumes all themes have accents, so we filter out the few that don't
                .Select(x => new ThemeColorViewModel(new Theme(x))));
            ThemeColors.Add(new ThemeColorViewModel(GetThemeFromCode()));
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/CustomTheme;component/CustomXamlTheme.xaml")
            };
            ThemeColors.Add(new ThemeColorViewModel(GetThemeFromDictionary("Jungle", rd)));

            SetThemeCommand = new RelayCommand<ThemeColorViewModel>(OnSetTheme);
        }

        private void OnSetTheme(ThemeColorViewModel theme)
        {
            if (theme == null) throw new ArgumentNullException(nameof(theme));

            _PaletteHelper.ReplacePalette(theme.Theme.GetPalette());
        }

        //TODO: This could easily be an extension method
        private static Theme GetThemeFromDictionary(string name, ResourceDictionary resourceDictionary)
        {
            return new Theme(name)
            {
                //NB: The names of these hues need to be set to these values to match what is in the MDIX themes
                //In addition these names are also used in the App.xaml to map to the MahApps brushes
                PrimaryLightHue = new Hue("Primary200", GetColor("PrimaryHueLight"), GetColor("PrimaryHueLightForeground")),
                PrimaryMidHue = new Hue("Primary500", GetColor("PrimaryHueMid"), GetColor("PrimaryHueMidForeground")),
                PrimaryDarkHue = new Hue("Primary700", GetColor("PrimaryHueDark"), GetColor("PrimaryHueDarkForeground")),
                SecondaryAccentHue = new Hue("Accent700", GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground")),
            };

            Color GetColor(string key)
            {
                var rv = resourceDictionary[key];
                if (rv == null) throw new InvalidOperationException($"Could not find '{key}' in resource dictionary");
                if (rv is Color color)
                {
                    return color;
                }

                throw new InvalidOperationException($"'{name}' is not a {nameof(Color)}");
            }
        }

        private static Theme GetThemeFromCode()
        {
            return new Theme("Fire and Ice")
            {
                //NB: The names of these hues need to be set to these values to match what is in the MDIX themes
                //In addition these names are also used in the App.xaml to map to the MahApps brushes
                PrimaryLightHue = new Hue("Primary200", Colors.LightSalmon, Colors.Black),
                PrimaryMidHue = new Hue("Primary500", Colors.Orange, Colors.Black),
                PrimaryDarkHue = new Hue("Primary700", Colors.Red, Colors.White),
                SecondaryAccentHue = new Hue("Accent700", Colors.DeepSkyBlue, Colors.Green)
            };
        }
    }

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