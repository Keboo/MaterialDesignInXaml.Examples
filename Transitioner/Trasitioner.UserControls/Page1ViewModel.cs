using GalaSoft.MvvmLight;

namespace Trasitioner.UserControls
{
    public class Page1ViewModel : ViewModelBase, ITransitionerViewModel
    {
        private string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set => Set(ref _FirstName, value);
        }

        private string _LastName;
        public string LastName
        {
            get => _LastName;
            set => Set(ref _LastName, value);
        }

        public void Hidden(ITransitionerViewModel newViewModel)
        {
            
        }

        public void Shown(ITransitionerViewModel previousViewModel)
        {

        }
    }
}