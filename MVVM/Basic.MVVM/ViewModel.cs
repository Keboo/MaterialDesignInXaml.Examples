using System.Windows.Input;

namespace Basic.MVVM
{
    public class ViewModel : ViewModelBase
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private readonly DelegateCommand _changeNameCommand;
        public ICommand ChangeNameCommand => _changeNameCommand;

        public ViewModel()
        {
            _changeNameCommand = new DelegateCommand(OnChangeName, CanChangeName);
        }

        private void OnChangeName(object commandParameter)
        {
            FirstName = "Walter";
            _changeNameCommand.InvokeCanExecuteChanged();
        }

        private bool CanChangeName(object commandParameter)
        {
            return FirstName != "Walter";
        }
    }
}