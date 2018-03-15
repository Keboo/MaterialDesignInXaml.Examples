using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;

namespace MVVM.Validation
{
    public class AttributeValidationViewModel : AnnotationValidationViewModel
    {
        private string _FirstName;
        [Required(ErrorMessage = "A first name is required")]
        [MinLength(3, ErrorMessage = "The first name must be at least 5 characters")]
        public string FirstName
        {
            get => _FirstName;
            set => Set(ref _FirstName, value);
        }

        private string _LastName;
        [Required]
        public string LastName
        {
            get => _LastName;
            set => Set(ref _LastName, value);
        }

        public RelayCommand SubmitCommand { get; }

        public AttributeValidationViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            //Doing this will cause the errors to show immediately
            ValidateModel();
        }

        private bool CanSubmit()
        {
            //Link the CanExecute state of the command to the visible errors on the screen. 
            //You can also separate the command from the validation errors and simply change this to match the one in SimpleViewModel.CanSubmit
            return !HasErrors;
        }

        private void OnSubmit()
        {
            Debug.WriteLine("Form Submitted");
        }
    }
}