using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChangePrimaryColor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Purple_Click(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.MediumPurple);
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.LightGreen);
        }

        private static void SetPrimaryColor(Color color)
        {
            //This is the API in 4.2.1
            PaletteHelper paletteHelper = new PaletteHelper();
            //For versions after this point use MaterialDesignThemes.Wpf.Theming.ThemeManager
            
            //Get current theme
            var theme = paletteHelper.GetTheme();

            //Apply primary color
            theme.SetPrimaryColor(color);

            //Apply theme to application
            paletteHelper.SetTheme(theme);
        }
    }
}
