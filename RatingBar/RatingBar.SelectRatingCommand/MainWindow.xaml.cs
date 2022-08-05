namespace RatingBar.SelectRatingCommand;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void RatingBar_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<int> e)
    {
        Output.Text = $"Value changed to {e.NewValue}";
    }
}
