using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace Snackbar.MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();

            Messenger.Default.Register<ShowMessage>(this, OnShowMessage);
        }

        private void OnShowMessage(ShowMessage message)
        {
            Snackbar.MessageQueue.Enqueue(message.Message, "Close", () => {
                Debug.WriteLine("Close clicked");
            });
        }
    }
}
