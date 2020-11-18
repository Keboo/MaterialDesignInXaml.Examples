using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ListBox.MultiSelectItems
{
    public class MainWindowViewModel
    {
        public ObservableCollection<Item> Items { get; } = new();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainWindowViewModel()
        {
            Items.Add(new Item { Name = "Breakfast" });
            Items.Add(new Item { Name = "Lunch" });
            Items.Add(new Item { Name = "Dinner" });

            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand<Item>(OnDelete);
        }

        private void OnDelete(Item item)
        {
            Items.Remove(item);
        }

        private async void OnAdd()
        {
            if (await DialogHost.Show(new Item()) is Item item)
            {
                Items.Add(item);
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
    }
}
