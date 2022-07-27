using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TestData;

namespace DataGrid.Selection;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Person> People { get; }

    public MainWindowViewModel()
    {
        People = new(Data.GeneratePeople(100));
    }

    private Person? _selectedPerson;
    public Person? SelectedPerson
    {
        get => _selectedPerson;
        set => SetProperty(ref _selectedPerson, value);
    }
}
