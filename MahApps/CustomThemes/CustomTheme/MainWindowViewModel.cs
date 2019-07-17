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
using Theme = MaterialDesignThemes.Wpf.Theme;

namespace CustomTheme
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PaletteHelper _PaletteHelper = new PaletteHelper();

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
                    ITheme theme = _PaletteHelper.GetTheme();
                    theme.SetBaseTheme(value ? Theme.Light : Theme.Dark);
                    _PaletteHelper.SetTheme(theme);
                }
            }
        }

        public MainWindowViewModel()
        {
            ThemeColors.AddRange(Enum.GetNames(typeof(MaterialDesignColor))
                .Where(x => Enum.TryParse<MaterialDesignColor>($"{x}Secondary", out _))
                .Select(x =>
                {
                    var primary = (MaterialDesignColor)Enum.Parse(typeof(MaterialDesignColor), x);
                    var secondary = (MaterialDesignColor)Enum.Parse(typeof(MaterialDesignColor), $"{x}Secondary");
                    Theme theme = Theme.Create(Theme.Light,
                        SwatchHelper.Lookup[primary],
                        SwatchHelper.Lookup[secondary]);
                    return new ThemeColorViewModel(theme, x);
                }));

            ThemeColors.Add(new ThemeColorViewModel(GetThemeFromCode(), "Fire and Ice"));
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/CustomTheme;component/CustomXamlTheme.xaml")
            };
            IThemeManager themeManager = Application.Current.Resources.GetThemeManager();
            themeManager.ThemeChanged += ThemeManagerOnThemeChanged;

            ThemeColors.Add(new ThemeColorViewModel(GetThemeFromDictionary(rd), "Jungle"));

            SetThemeCommand = new RelayCommand<ThemeColorViewModel>(OnSetTheme);
        }

        private void ThemeManagerOnThemeChanged(object sender, ThemeChangedEventArgs e)
        {

        }

        private void OnSetTheme(ThemeColorViewModel themeVm)
        {
            if (themeVm == null) throw new ArgumentNullException(nameof(themeVm));

            ITheme theme = themeVm.Theme;
            theme.SetBaseTheme(IsLightTheme ? Theme.Light : Theme.Dark);
            _PaletteHelper.SetTheme(theme);
        }

        private static ITheme GetThemeFromDictionary(ResourceDictionary resourceDictionary)
        {
            var theme = new Theme();
            theme.SetBaseTheme(Theme.Light);

            theme.PrimaryLight = new ColorPair(GetColor("PrimaryHueLight"), GetColor("PrimaryHueLightForeground"));
            theme.PrimaryMid = new ColorPair(GetColor("PrimaryHueMid"), GetColor("PrimaryHueMidForeground"));
            theme.PrimaryDark = new ColorPair(GetColor("PrimaryHueDark"), GetColor("PrimaryHueDarkForeground"));

            theme.SecondaryLight = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));
            theme.SecondaryMid = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));
            theme.SecondaryDark = new ColorPair(GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground"));

            return theme;

            //return new Theme(name)
            //{
            //    //NB: The names of these hues need to be set to these values to match what is in the MDIX themes
            //    //In addition these names are also used in the App.xaml to map to the MahApps brushes
            //    PrimaryLightHue = new Hue("Primary200", GetColor("PrimaryHueLight"), GetColor("PrimaryHueLightForeground")),
            //    PrimaryMidHue = new Hue("Primary500", GetColor("PrimaryHueMid"), GetColor("PrimaryHueMidForeground")),
            //    PrimaryDarkHue = new Hue("Primary700", GetColor("PrimaryHueDark"), GetColor("PrimaryHueDarkForeground")),
            //    SecondaryAccentHue = new Hue("Accent700", GetColor("SecondaryAccent"), GetColor("SecondaryAccentForeground")),
            //};

            Color GetColor(string key)
            {
                var rv = resourceDictionary[key];
                if (rv == null) throw new InvalidOperationException($"Could not find '{key}' in resource dictionary");
                if (rv is Color color)
                {
                    return color;
                }

                throw new InvalidOperationException($"'{key}' is not a {nameof(Color)}");
            }
        }

        private static ITheme GetThemeFromCode()
        {
            var theme = new Theme();
            theme.SetBaseTheme(Theme.Light);

            theme.PrimaryLight = new ColorPair(Colors.LightSalmon);
            theme.PrimaryMid = new ColorPair(Colors.Orange);
            theme.PrimaryDark = new ColorPair(Colors.Red);

            theme.SecondaryLight = new ColorPair(Colors.DeepSkyBlue, Colors.Green);
            theme.SecondaryMid = new ColorPair(Colors.DeepSkyBlue, Colors.Green);
            theme.SecondaryDark = new ColorPair(Colors.DeepSkyBlue, Colors.Green);

            return theme;

            //return new Theme("Fire and Ice")
            //{
            //    //NB: The names of these hues need to be set to these values to match what is in the MDIX themes
            //    //In addition these names are also used in the App.xaml to map to the MahApps brushes
            //    PrimaryLightHue = new Hue("Primary200", Colors.LightSalmon, Colors.Black),
            //    PrimaryMidHue = new Hue("Primary500", Colors.Orange, Colors.Black),
            //    PrimaryDarkHue = new Hue("Primary700", Colors.Red, Colors.White),
            //    SecondaryAccentHue = new Hue("Accent700", Colors.DeepSkyBlue, Colors.Green)
            //};
        }
    }
}