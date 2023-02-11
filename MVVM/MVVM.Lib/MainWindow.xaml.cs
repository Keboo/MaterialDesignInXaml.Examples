using MVVM.Lib.Shared;

namespace MVVM.Lib;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = new SharedViewModel();
        InitializeComponent();
    }
}
