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

namespace DialogHost.CodeBehindExample.Dialogs
{
    /// <summary>
    /// Interaction logic for TextInputDialog.xaml
    /// </summary>
    public partial class TextInputDialog : UserControl
    {
        public TextInputDialog(TextInputDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }

    public class TextInputDialogViewModel : ViewModelBase
    {
        public string Message { get; set; } = "Enter some text";
        public string AffirmativeButtonText { get; set; } = "OK";
        public string NegativeButtonText { get; set; } = "CANCEL";

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }
    }
}
