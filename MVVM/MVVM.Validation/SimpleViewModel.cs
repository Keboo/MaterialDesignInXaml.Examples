using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace MVVM.Validation
{
    public class SimpleViewModel : ViewModelBase
    {
        private string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set => Set(ref _FirstName, value);
        }

        private string _LastName;
        public string LastName
        {
            get => _LastName;
            set => Set(ref _LastName, value);
        }

        
        public RelayCommand SubmitCommand { get; }

        public SimpleViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
        }

        private bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        private void OnSubmit()
        {
            Debug.WriteLine("Form Submitted");
        }
    }
}