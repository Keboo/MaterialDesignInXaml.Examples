using DialogHost.ChangingContent.CodeBehind;
using System.Diagnostics;
using System.Windows;

namespace DialogHost.ChangingContent
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

        private async void CodeBehind_Click(object sender, RoutedEventArgs e)
        {
            var loginControl = new LoginControl(RootDialogHost);

            object result = await MaterialDesignThemes.Wpf.DialogHost.Show(loginControl, RootDialogHost.Identifier);

            Debug.WriteLine($"Dialog Result {result}");
        }
    }
}
