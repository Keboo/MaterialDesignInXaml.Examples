using System.Windows.Input;

namespace Menu.ToolBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static RoutedCommand MenuItemSelected { get; } = new RoutedCommand("MenuItemSelected", typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedMenuTextBlock.Text = $"Selected Item: {e.Parameter}";
        }
    }
}
