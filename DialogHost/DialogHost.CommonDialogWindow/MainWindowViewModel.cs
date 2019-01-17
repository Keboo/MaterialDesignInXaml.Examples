using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace DialogHost.CommonDialogWindow
{
    public class MainWindowViewModel
    {
        public ICommand ShowDialogCommand { get; }
        public ICommand ShowMainWindowCommand { get; }
        public ICommand ShowOtherWindowCommand { get; }

        public MainWindowViewModel()
        {
            ShowDialogCommand = new RelayCommand<string>(OnShowDialog);
            ShowMainWindowCommand = new RelayCommand(OnShowMainWindow);
            ShowOtherWindowCommand = new RelayCommand(OnShowOtherWindow);
        }

        private static async void OnShowDialog(string dialogIdentifier)
        {
            //We are making the assumption that the dialog identifier is passed into the command
            await MaterialDesignThemes.Wpf.DialogHost.Show(new Dialog(), dialogIdentifier);
        }

        private static void OnShowMainWindow()
        {
            new MainWindow().Show();
        }

        private static void OnShowOtherWindow()
        {
            new OtherWindow().Show();
        }
    }
}