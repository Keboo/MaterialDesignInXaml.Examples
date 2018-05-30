using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DialogHost.ClosingConfirmation.CodeBehind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool _ShowingDialog;
        private bool _AllowClose;

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            //If the user has elected to allow the close, simply let the closing event happen.
            if (_AllowClose) return;

            //NB: Because we are making an async call we need to cancel the closing event
            e.Cancel = true;

            //we are already showing the dialog, ignore
            if (_ShowingDialog) return;

            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "Are you sure?";

            Button btn1 = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn1.Style = style;
            btn1.Width = 115;
            btn1.Height = 30;
            btn1.Margin = new Thickness(5);
            btn1.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn1.CommandParameter = true;
            btn1.Content = "Yes";

            Button btn2 = new Button();
            Style style2 = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn2.Style = style2;
            btn2.Width = 115;
            btn2.Height = 30;
            btn2.Margin = new Thickness(5);
            btn2.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn2.CommandParameter = false;
            btn2.Content = "No";


            DockPanel dck = new DockPanel();
            dck.Children.Add(btn1);
            dck.Children.Add(btn2);

            StackPanel stk = new StackPanel();
            stk.Width = 250;
            stk.Children.Add(txt1);
            stk.Children.Add(dck);
            
            //Set flag indicating that the dialog is being shown
            _ShowingDialog = true;
            object result = await MaterialDesignThemes.Wpf.DialogHost.Show(stk);
            _ShowingDialog = false;
            //The result returned will come form the button's CommandParameter.
            //If the user clicked "Yes" set the _AllowClose flag, and re-trigger the window Close.
            if (result is bool boolResult && boolResult)
            {
                _AllowClose = true;
                Close();
            }
        }
    }
}
