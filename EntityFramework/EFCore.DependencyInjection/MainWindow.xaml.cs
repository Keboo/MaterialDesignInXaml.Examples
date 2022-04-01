using System;
using System.Windows;

namespace EFCore.DependencyInjection;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        InitializeComponent();
    }
}
