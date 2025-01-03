using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;

namespace LibWithMaterialDesign;

public partial class LibraryWindowViewModel : ObservableObject
{
    private IThemeSwitcher ThemeSwitcher { get; }

    public LibraryWindowViewModel(IThemeSwitcher themeSwitcher)
    {
        ThemeSwitcher = themeSwitcher;
    }


    [RelayCommand]
    private void ChangePrimaryColor()
    {
        Random random = Random.Shared;

        Color newColor = Color.FromRgb(
            (byte)random.Next(byte.MaxValue),
            (byte)random.Next(byte.MaxValue),
            (byte)random.Next(byte.MaxValue)
        );

        ThemeSwitcher.ChangePrimaryColor(newColor);
    }
}
