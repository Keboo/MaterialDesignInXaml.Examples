using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogHost.RoutedCommand
{
    /// <summary>
    /// Interaction logic for SimpleDialog.xaml
    /// </summary>
    public partial class SimpleDialog : UserControl
    {
        public SimpleDialog()
        {
            InitializeComponent();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            CloseDialog();
        }

        private void CloseDialog()
        {
            var command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            if (command.CanExecute(null, this))
            {
                command.Execute(null, this);
            }
        }
    }
}
