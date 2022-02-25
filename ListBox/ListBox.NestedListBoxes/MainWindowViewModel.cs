using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TestData;

namespace ListBox.NestedListBoxes;


internal class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Team> Teams { get; } = new();

    public MainWindowViewModel()
    {
        foreach (var team in Data.GenerateTeams(5, 4))
        {
            Teams.Add(team);
        }
    }
}
