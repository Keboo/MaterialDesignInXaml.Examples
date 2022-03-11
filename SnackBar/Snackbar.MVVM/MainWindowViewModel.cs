using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;

namespace Snackbar.MVVM
{
    public class MainWindowViewModel : ViewModelBase
    {
        public SnackbarMessageQueue BoundMessageQueue { get; } = new SnackbarMessageQueue();

        public RelayCommand<string> SendMessageCommand { get; }
        public RelayCommand<string> SendWithPubSubCommand { get; }

        public MainWindowViewModel()
        {
            SendMessageCommand = new RelayCommand<string>(OnSendMessage);
            SendWithPubSubCommand = new RelayCommand<string>(OnSendWithPubSub);
        }

        private void OnSendWithPubSub(string message)
        {
            //This is recived in the code behind on the MainWindow.xaml.cs
            MessengerInstance.Send(new ShowMessage(message));
        }

        private void OnSendMessage(string message)
        {
            BoundMessageQueue.Enqueue(message, "Close", () =>
            {
                Debug.WriteLine("Close clicked");
            });
        }
    }

    public class ShowMessage
    {
        public ShowMessage(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}