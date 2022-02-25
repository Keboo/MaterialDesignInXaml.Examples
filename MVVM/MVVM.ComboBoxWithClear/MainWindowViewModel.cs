using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVM.ComboBoxWithClear;

public record class Component(string Location)
{ }

public record class Material(string Name)
{ }

public class MainWindowViewModel : ObservableObject
{
    private Component? _selectedComponent;
    public Component? SelectedComponent
    {
        get => _selectedComponent;
        set => SetProperty(ref _selectedComponent, value);
    }

    private ObservableCollection<Component> _stationBaseComponentList = new();
    public ObservableCollection<Component> StationBaseComponentList 
    {
        get => _stationBaseComponentList;
        set => SetProperty(ref _stationBaseComponentList, value);
    }

    private ObservableCollection<Material> _materialsList = new();
    public ObservableCollection<Material> MaterialsList
    {
        get => _materialsList;
        set => SetProperty(ref _materialsList, value);
    }

    private Material? _selectedMaterial;
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set => SetProperty(ref _selectedMaterial, value);
    }

    private RelayCommand? _clearListCommand;
    public ICommand ClearListCommand => _clearListCommand ??= new RelayCommand(ClearList);

    private void ClearList()
    {
        SelectedComponent = null;
        SelectedMaterial = null;
    }

    public MainWindowViewModel()
    {
        StationBaseComponentList.Add(new Component("Location 1"));
        StationBaseComponentList.Add(new Component("Location 2"));
        StationBaseComponentList.Add(new Component("Location 3"));
        StationBaseComponentList.Add(new Component("Location 4"));
        StationBaseComponentList.Add(new Component("Location 5"));
        StationBaseComponentList.Add(new Component("Location 6"));
        StationBaseComponentList.Add(new Component("Location 7"));
        StationBaseComponentList.Add(new Component("Location 8"));

        MaterialsList.Add(new Material("Material 1"));
        MaterialsList.Add(new Material("Material 2"));
        MaterialsList.Add(new Material("Material 3"));
        MaterialsList.Add(new Material("Material 4"));
        MaterialsList.Add(new Material("Material 5"));
    }


}
