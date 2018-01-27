using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
}