using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TabControl.MVVM;

[ObservableObject]
public partial class MainWindowViewModel
{
    private int _counter = 1;
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
        TabItemViewModel newTab;
        if (_counter % 2 == 0)
        {
            newTab = new OtherTabViewModel();
        }
        else
        {
            newTab = new TabItemViewModel();
        }
        newTab.Title = $"Tab {_counter++}";
        Tabs.Add(newTab);
    }

    [RelayCommand]
    private void CloseTab(TabItemViewModel tab)
    {
        Tabs.Remove(tab);
    }
}
