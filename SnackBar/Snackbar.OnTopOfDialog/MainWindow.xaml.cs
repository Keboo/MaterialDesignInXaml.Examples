using System.Windows;

namespace Snackbar.OnTopOfDialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowMessage_OnClick(object sender, RoutedEventArgs e)
        {
            Snackbar.MessageQueue.Enqueue("Snackbar Message!");
        }
    }
}
