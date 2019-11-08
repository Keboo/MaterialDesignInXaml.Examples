namespace DialogHost.ChangingContent.MVVM.ViewModel
{
    public class TransitionViewModel : ViewModelBase
    {
        private string _Username;
        public string Username
        {
            get => _Username;
            set => SetProperty(ref _Username, value);
        }

        //TODO: More stuff to support the LoginControl and CreateAccountControl in the Transition namespace
    }
}
