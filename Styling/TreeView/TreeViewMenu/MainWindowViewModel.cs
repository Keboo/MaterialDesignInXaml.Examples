using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace TreeViewMenu
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ItemSelectedCommand { get; }

        private object _SelectedItem;
        public object SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }

        public MainWindowViewModel()
        {
            ItemSelectedCommand = new RelayCommand<object>(OnItemSelected);
        }

        private void OnItemSelected(object selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}