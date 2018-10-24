using MaterialDesignThemes.Wpf;
using PersistThemePreferences.Properties;
using System;
using System.Windows;

namespace PersistThemePreferences
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetTheme(Settings.Default.Theme);
            base.OnStartup(e);
        }

        public void SetTheme(BaseTheme theme)
        {
            //Copied from the existing ThemeAssist class
            //https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/MaterialDesignThemes.Wpf/ThemeAssist.cs

            string lightSource =  "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";
            string darkSource = "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml";

            foreach (ResourceDictionary resourceDictionary in Resources.MergedDictionaries)
            {
                if (string.Equals(resourceDictionary.Source?.ToString(), lightSource, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(resourceDictionary.Source?.ToString(), darkSource, StringComparison.OrdinalIgnoreCase))
                {
                    Resources.MergedDictionaries.Remove(resourceDictionary);
                    break;
                }
            }

            if (theme == BaseTheme.Dark)
            {
                Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri(darkSource) });
            }
            else
            {
                //This handles both Light and Inherit
                Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri(lightSource) });
            }
        }
    }
}
