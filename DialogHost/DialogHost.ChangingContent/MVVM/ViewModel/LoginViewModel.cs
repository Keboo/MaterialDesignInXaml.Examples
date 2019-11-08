using System;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace DialogHost.ChangingContent.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public DialogSession DialogSession { get; set; }

        public ICommand ShowCreateAccount { get; }

        private string _Username;
        public string Username
        {
            get => _Username;
            set => SetProperty(ref _Username , value);
        }

        public LoginViewModel()
        {
            ShowCreateAccount = new DelegateCommand(OnShowCreateAccount);
        }

        private void OnShowCreateAccount(object _)
        {
            DialogSession?.UpdateContent(new CreateAccountViewModel(DialogSession));
        }
    }
}
