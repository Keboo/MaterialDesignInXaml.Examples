using PersistThemePreferences.Properties;
using System.Windows;

namespace PersistThemePreferences
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Toggle.IsChecked = Settings.Default.Theme == MaterialDesignThemes.Wpf.BaseTheme.Dark;
            SetTheme();
        }

        private void Toggle_Checked(object sender, RoutedEventArgs e) => SetTheme();

        private void Toggle_Unchecked(object sender, RoutedEventArgs e) => SetTheme();

        private void SetTheme()
        {
            if (Toggle.IsChecked == true)
            {
                Text.Text = "Dark Theme";
                Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Dark;
            }
            else
            {
                Text.Text = "Light Theme";
                Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Light;
            }

            Settings.Default.Save();
            ((App)Application.Current).SetTheme(Settings.Default.Theme);
        }
    }
}
