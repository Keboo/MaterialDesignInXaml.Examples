using GalaSoft.MvvmLight;

namespace Trasitioner.UserControls
{
    public class Page2ViewModel : ViewModelBase, ITransitionerViewModel
    {
        private string _City;
        public string City
        {
            get => _City;
            set => Set(ref _City, value);
        }

        public void Hidden(ITransitionerViewModel newViewModel)
        {
            
        }

        public void Shown(ITransitionerViewModel previousViewModel)
        {
            
        }
    }
}