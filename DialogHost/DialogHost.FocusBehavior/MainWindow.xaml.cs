using System.Windows;
using System.Windows.Input;

namespace DialogHost.FocusBehavior
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            Keyboard.AddGotKeyboardFocusHandler(this, Handler);
            InitializeComponent();
        }

        private void Handler(object sender, KeyboardFocusChangedEventArgs e)
        {
            FocusedElement.Text = (e.NewFocus as FrameworkElement)?.Name;
        }
    }
}
