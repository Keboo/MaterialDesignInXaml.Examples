using System;
using MaterialDesignThemes.Wpf;

namespace DialogHost.AttachedPropertyClosing
{
    public class MainWindowViewModel
    {
        public DialogClosingEventHandler DialogClosingHandler { get; }

        public MainWindowViewModel()
        {
            DialogClosingHandler = OnDialogClosing;
        }

        private void OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            //TODO: Handle closing
        }
    }
}
