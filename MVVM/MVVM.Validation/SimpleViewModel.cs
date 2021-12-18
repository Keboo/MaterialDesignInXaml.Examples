using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;

namespace MVVM.Validation
{
    public class SimpleViewModel : ObservableObject
    {
        private string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set => SetProperty(ref _FirstName, value);
        }

        private string _LastName;
        public string LastName
        {
            get => _LastName;
            set => SetProperty(ref _LastName, value);
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