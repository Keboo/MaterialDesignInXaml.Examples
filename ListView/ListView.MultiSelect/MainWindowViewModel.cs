using TestData;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using CommunityToolkit.Mvvm.Input;

namespace ListView.MultiSelect;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        Items = [.. Data.GeneratePeople(1_000)];
    }
    public ObservableCollection<Person> Items { get; } = [];

    [ObservableProperty]
    private IList? _selectedItems;

    partial void OnSelectedItemsChanged(IList? oldValue, IList newValue)
    {

    }

    [RelayCommand]
    private void OnSelectTopTen()
    {
        SelectedItems = Items.Take(10).ToList();
    }
}
