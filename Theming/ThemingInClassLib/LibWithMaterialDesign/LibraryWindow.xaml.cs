using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace LibWithMaterialDesign;

/// <summary>
/// Interaction logic for LibraryWindow.xaml
/// </summary>
public partial class LibraryWindow : Window, IThemeSwitcher
{
    public LibraryWindow()
    {
        DataContext = new LibraryWindowViewModel(this);
        InitializeComponent();
    }

    private void ToggleThemeClick(object sender, RoutedEventArgs e)
    {
        ResourceDictionary themeResourceDictionary = GetThemeResourceDictionary();
        Theme theme = themeResourceDictionary.GetTheme();

        BaseTheme currentTheme = theme.GetBaseTheme();
        BaseTheme newTheme = currentTheme switch
        {
            BaseTheme.Light => BaseTheme.Dark,
            _ => BaseTheme.Light
        };
        theme.SetBaseTheme(newTheme);
        
        themeResourceDictionary.SetTheme(theme);
    }

    private ResourceDictionary GetThemeResourceDictionary()
    {
        //We can't use PaletteHelper here because it will try to use Application.Current.Resource
        //Instead we need to give it the resource dictionary that contains the material design theme dictionaries.
        //In this case that is the theme dictionary inside of this Window's Resources.
        return Resources.MergedDictionaries.Single(x => x is IMaterialDesignThemeDictionary);
    }

    public void ChangePrimaryColor(Color primaryColor)
    {
        ResourceDictionary themeResourceDictionary = GetThemeResourceDictionary();
        Theme theme = themeResourceDictionary.GetTheme();

        theme.SetPrimaryColor(primaryColor);

        themeResourceDictionary.SetTheme(theme);
    }
}
