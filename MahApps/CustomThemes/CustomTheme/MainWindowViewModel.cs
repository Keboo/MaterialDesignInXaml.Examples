using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
                    _PaletteHelper.SetLightDark(!value);
                }
            }
        }

        public MainWindowViewModel()
        {
            var swatchProvider = new SwatchesProvider();
            ThemeColors.AddRange(swatchProvider.Swatches
                .Where(x => x.IsAccented) //This sample assumes all colors have accents
                .Select(x => new ThemeColorViewModel(new Theme(x))));
            //ThemeColors.Add(new ThemeColorViewModel(CustomCodeTheme.GetSwatch()));
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/CustomTheme;component/CustomXamlTheme.xaml")
            };
            //ThemeColors.Add(new ThemeColorViewModel(GetSwatchFromDictionary(rd)));

            SetThemeCommand = new RelayCommand<ThemeColorViewModel>(OnSetTheme);
        }

        private void OnSetTheme(ThemeColorViewModel theme)
        {
            if (theme == null) throw new ArgumentNullException(nameof(theme));

            _PaletteHelper.ReplacePalette(theme.Theme.GetPalette());
        }

        //TODO: This could easily be an extension method
        private static Theme GetThemeFromDictionary(ResourceDictionary resourceDictionary)
        {
            return null;
        }

        private static Theme GetThemeFromCode()
        {
            return null;
        }
    }
}