using LibWithMaterialDesign;
using System.Windows;

namespace HostApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LaunchLibraryWindowClick(object sender, RoutedEventArgs e)
    {
        //This is a hard reference for the example. Most plug-in systems will be more dynamic.
        LibraryWindow window = new();
        window.Show();
    }
}