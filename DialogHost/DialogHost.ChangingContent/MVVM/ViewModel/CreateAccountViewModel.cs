using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Input;

namespace DialogHost.ChangingContent.MVVM.ViewModel
{
    public class CreateAccountViewModel
    {
        public DialogSession DialogSession { get; }

        public ICommand BackCommand { get; }

        public CreateAccountViewModel(DialogSession dialogSession)
        {
            DialogSession = dialogSession ?? throw new ArgumentNullException(nameof(dialogSession));
            BackCommand = new DelegateCommand(OnBack);
        }

        private void OnBack(object _)
        {
            DialogSession.UpdateContent(new LoginViewModel());
        }
    }
}
