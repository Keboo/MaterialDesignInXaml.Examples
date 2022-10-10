using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TestData;

namespace DataGrid.NumericPicker;

[ObservableObject]
internal partial class MainWindowViewModel
{
    public ObservableCollection<Product> Products { get; } = new();

	public MainWindowViewModel()
	{
        foreach(Product product in Data.GenerateProducts(50))
        {
            Products.Add(product);
        }
	}
}
