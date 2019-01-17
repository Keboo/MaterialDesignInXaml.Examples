using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace DialogHost.CommonDialogWindow
{
    public class OtherWindowViewModel
    {
        //Rather than passing in the dialog host's identifier we will have the view model supply it
        public string DialogHostIdentifier { get; } = Guid.NewGuid().ToString();

        public ICommand ShowDialogCommand { get; }

        public OtherWindowViewModel()
        {
            ShowDialogCommand = new RelayCommand(OnShowDialog);
        }

        private async void OnShowDialog()
        {
            await MaterialDesignThemes.Wpf.DialogHost.Show(new Dialog(), DialogHostIdentifier);
        }
    }
}