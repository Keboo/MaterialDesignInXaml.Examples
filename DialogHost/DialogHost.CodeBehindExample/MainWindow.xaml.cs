using DialogHost.CodeBehindExample.Dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

namespace DialogHost.CodeBehindExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
        }

        private async void GroupAButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OkCancelDialog();
            var result = (bool)await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "GroupDialogHost");

            Snackbar.MessageQueue.Enqueue($"{(result ? "OK" : "CANCEL")} Button was clicked");
        }

        private async void GroupBButtonClick(object sender, RoutedEventArgs e)
        {
            var viewModel = new TextInputDialogViewModel
            {
                Message = "Enter some text"
            };

            var dialog = new TextInputDialog(viewModel);
            var result = (bool)await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost");
            if (result)
            {
                Snackbar.MessageQueue.Enqueue($"Received text: {viewModel.Text}");
            }
        }

        private async void GroupCButtonClick(object sender, RoutedEventArgs e)
        {
            var viewModel = new TextInputDialogViewModel
            {
                AffirmativeButtonText = "TRY NOW",
                NegativeButtonText = "NEVERMIND",
                Message = "Type 'cool' into the box"
            };

            var dialog = new TextInputDialog(viewModel);
            await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "MainDialogHost", DialogClosing);
            Snackbar.MessageQueue.Enqueue($"We did it!");
        }

        private void DialogClosing(object sender, DialogClosingEventArgs e)
        {
            if (!(bool)e.Parameter)
            {
                //cancel button was clicked, don't do any of this logic
                return;
            }

            //This will get called when the dialog is about to close
            if (e.Session.Content is TextInputDialog dialog)
            {
                if (dialog.DataContext is TextInputDialogViewModel viewModel)
                {
                    if (viewModel.Text?.ToLower() != "cool")
                    {
                        viewModel.Text = "NO!";
                        e.Cancel(); //prevents the dialog from closing
                    }
                }
            }
        }
    }
}
