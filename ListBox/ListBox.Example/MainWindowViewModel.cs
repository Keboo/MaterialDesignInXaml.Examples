using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ListBox.Example
{
    public class MainWindowViewModel
    {
        public ObservableCollection<ItemViewModel> Items { get; } = new ObservableCollection<ItemViewModel>();

        public MainWindowViewModel()
        {
            for (int i = 1; i < 20; i++)
            {
                Items.Add(new ItemViewModel($"Item {i}"));
            }
        }
    }
}
