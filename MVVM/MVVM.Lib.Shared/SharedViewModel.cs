using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MVVM.Lib.Shared;


public partial class SharedViewModel : ObservableObject
{
    [ObservableProperty]
    private int _count;

    [RelayCommand]
    public void Increment()
    {
        Count++;
    }
}
