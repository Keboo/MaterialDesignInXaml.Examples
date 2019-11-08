using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace DialogHost.ChangingContent.MVVM.ViewModel
{
    public class CreateAccountViewModel : ViewModelBase
    {
        public DialogSession DialogSession { get; }

        public ICommand BackCommand { get; }

        private string _Username;
        public string Username
        {
            get => _Username;
            set => SetProperty(ref _Username, value);
        }

        public CreateAccountViewModel(DialogSession dialogSession)
        {
            DialogSession = dialogSession ?? throw new ArgumentNullException(nameof(dialogSession));
            BackCommand = new DelegateCommand(OnBack);
        }

        private void OnBack(object _)
        {
            DialogSession.UpdateContent(new LoginViewModel 
            {
                Username = Username
            });
        }
    }
}
