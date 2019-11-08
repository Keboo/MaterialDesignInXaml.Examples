using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace DialogHost.ChangingContent.MVVM.ViewModel
{
    public class MainWindowViewModel
    {
        private const string DialogHostId = "RootDialogHostId";

        public ICommand ShowLoginDialog { get; }

        public MainWindowViewModel()
        {
            ShowLoginDialog = new DelegateCommand(OnShowLoginDialog);
        }

        private async void OnShowLoginDialog(object _)
        {
            var viewModel = new LoginViewModel();
            await MaterialDesignThemes.Wpf.DialogHost.Show(viewModel, DialogHostId, new DialogOpenedEventHandler((sender, args) =>
            {
                viewModel.DialogSession = args.Session;
            }));
        }
    }
}
