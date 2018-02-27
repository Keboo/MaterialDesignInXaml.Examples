using GalaSoft.MvvmLight.Messaging;

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
            Snackbar.MessageQueue.Enqueue(message.Message);
        }
    }
}
