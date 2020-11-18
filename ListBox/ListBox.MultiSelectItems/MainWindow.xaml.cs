using System.Windows;
using System.Windows.Input;

namespace ListBox.MultiSelectItems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            vm.DeleteCommand.Execute(e.Parameter);
        }
    }
}
