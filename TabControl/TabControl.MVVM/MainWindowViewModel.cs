using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TabControl.MVVM;

[ObservableObject]
public partial class MainWindowViewModel
{
    public ObservableCollection<TabItemViewModel> Tabs { get; } = new();

    public MainWindowViewModel()
    {
        NewTab();
        NewTab();
        NewTab();
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
