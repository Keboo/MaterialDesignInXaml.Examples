using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace CustomTheme
{
    public class MainWindowViewModel
    {
        public List<ThemeColorViewModel> ThemeColors { get; } = new List<ThemeColorViewModel>();

        public ICommand SetThemeCommand { get; }

        public MainWindowViewModel()
        {
            var swatchProvider = new SwatchesProvider();
            ThemeColors.AddRange(swatchProvider.Swatches.Select(x => new ThemeColorViewModel(x)));
            //ThemeColors.Add(new ThemeColorViewModel(CustomCodeTheme.GetSwatch()));
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/CustomTheme;component/CustomXamlTheme.xaml")
            };
            //ThemeColors.Add(new ThemeColorViewModel(GetSwatchFromDictionary(rd)));

            SetThemeCommand = new RelayCommand<Swatch>(OnSetTheme);
        }

        private void OnSetTheme(Swatch swatch)
        {
            if (swatch == null) throw new ArgumentNullException(nameof(swatch));
            new PaletteHelper().ReplacePalette(new Palette(swatch, swatch, swatch.));
        }

        //TODO: This could easily be an extension method
        private static Swatch GetSwatchFromDictionary(ResourceDictionary resourceDictionary)
        {
            return null;
        }
    }
}