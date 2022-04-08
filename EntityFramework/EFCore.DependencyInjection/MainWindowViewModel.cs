using EFCore.DependencyInjection.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace EFCore.DependencyInjection;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Person> Customers { get; }

    public MainWindowViewModel(CustomerContext context)
    {
        context.Customers.Load();
        Customers = context.Customers.Local.ToObservableCollection();
    }

}

