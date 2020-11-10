using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace DialogHost.FromViewModel
{
    public class MainWindowViewModel
    {
        private const string DialogIdentifier = "RootDialogHost";

        public ICommand ShowLoginCommand { get; }
        public ICommand ShowCustomLoginCommand { get; }

        public ICommand ShowMessageCommand { get; }

        public MainWindowViewModel()
        {
            ShowLoginCommand = new RelayCommand(OnShowLogin);
            ShowMessageCommand = new RelayCommand<string>(OnShowMessage);
            ShowCustomLoginCommand = new RelayCommand(OnShowCustomLogin);
        }

        private async void OnShowLogin()
        {
            var vm = new LoginViewModel();
            object dialogResult = await MaterialDesignThemes.Wpf.DialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                string username = vm.Username;
                string password = vm.Password;
                //TODO: Do login stuff
            }
            else
            {
                //TODO: Do login canceled stuff
            }
        }

        private async void OnShowMessage(string message)
        {
            var vm = new MessageViewModel(message);
            await MaterialDesignThemes.Wpf.DialogHost.Show(vm, DialogIdentifier);
        }

        private async void OnShowCustomLogin()
        {
            var vm = new CustomLoginViewModel();
            object dialogResult = await MaterialDesignThemes.Wpf.DialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                string username = vm.Username;
                string password = vm.Password;
                //TODO: Do login stuff
            }
            else
            {
                //TODO: Do login canceled stuff
            }
        }
    }
}