using Microsoft.Toolkit.Mvvm.ComponentModel;
using MVVM.HamburgerNavigation.ViewModels;
using System.Collections.ObjectModel;

namespace MVVM.HamburgerNavigation
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<IMenuItem> MenuItems { get; }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    IsMenuOpen = false;
                }
            }
        }

        private bool isMenuOpen;
        public bool IsMenuOpen
        {
            get => isMenuOpen;
            set => SetProperty(ref isMenuOpen, value);
        }

        public MainWindowViewModel()
        {
            MenuItems = new()
            {
                new HomeViewModel(),
                new AnimalsViewModel(),
                new FoodViewModel(),
                new PlacesViewModel(),
                new ThingsViewModel(),
                new RandomViewModel()
            };
            SelectedItem = MenuItems[0];
        }
    }
}
