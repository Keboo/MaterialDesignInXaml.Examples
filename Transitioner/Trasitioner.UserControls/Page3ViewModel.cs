using GalaSoft.MvvmLight;

namespace Trasitioner.UserControls
{
    public class Page3ViewModel : ViewModelBase, ITransitionerViewModel
    {
        private string _Country;
        public string Country
        {
            get => _Country;
            set => Set(ref _Country, value);
        }

        public void Hidden(ITransitionerViewModel newViewModel)
        {
            
        }

        public void Shown(ITransitionerViewModel previousViewModel)
        {
            
        }
    }
}