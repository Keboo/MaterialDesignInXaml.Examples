using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace DialogHost.Example
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ShowLoginFormCommand { get; }
        
        public MainWindowViewModel()
        {
            ShowLoginFormCommand = new RelayCommand(OnShowLoginForm);
        }

        private async void OnShowLoginForm()
        {
            var vm = new LoginViewModel();
            await MDIXDialogHost.Show(vm, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }
    }
}