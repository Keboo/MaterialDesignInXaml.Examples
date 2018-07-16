using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DialogHost.WithResult
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ShowDialogCommand { get; }

        private string _Result;
        public string Result
        {
            get => _Result;
            set => Set(ref _Result, value);
        }

        public MainWindowViewModel()
        {
            ShowDialogCommand = new RelayCommand(OnShowDialog);
        }

        private async void OnShowDialog()
        {
            object result = await MaterialDesignThemes.Wpf.DialogHost.Show("Are dogs better than cats?");
            
            //Result will be null in the case where you click off of the dialog without picking an option
            Answer answer = (Answer)(result ?? Answer.None);
            switch (answer)
            {
                case Answer.Yes:
                    Result = "You're right! Dogs are better.";
                    break;
                case Answer.No:
                    Result = "Wrong! Dogs are better.";
                    break;
                default:
                    Result = "";
                    break;

            }
        }
    }
}