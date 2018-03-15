using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DrawerHost.Airspace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            WebBrowser.Navigate("http://www.bing.com");
        }

        private void GoogleOnClick(object sender, RoutedEventArgs e)
        {
            WebBrowser.Navigate("http://www.google.com");
            DrawerHost.IsLeftDrawerOpen = false;
        }

        private void BingOnClick(object sender, RoutedEventArgs e)
        {
            WebBrowser.Navigate("http://www.bing.com");
            DrawerHost.IsLeftDrawerOpen = false;
        }

        private void WebBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            WebSource.Text = e.Uri.ToString();
        }
    }
}
