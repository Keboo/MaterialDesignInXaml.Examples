using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TabControl.MVVM;

[ObservableObject]
public partial class MainWindowViewModel
{
    public ObservableCollection<TabItemViewModel> Tabs { get; } = new();
    public ObservableCollection<CustomItemViewModel> OtherTabs { get; } = new();

    public MainWindowViewModel()
    {
        NewTab();
        NewTab();
        NewTab();

        OtherTabs.Add(new Item1ViewModel());
        OtherTabs.Add(new Item2ViewModel());
    }

    [RelayCommand]
    private void NewTab()
    {
        Tabs.Add(new TabItemViewModel { Title = $"Tab {Tabs.Count + 1}" });
    }

    [RelayCommand]
    private void CloseTab(TabItemViewModel tab)
    {
        Tabs.Remove(tab);
    }
}
