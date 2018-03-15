using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace MVVM.Validation
{
    public class NotifyErrorInfoViewModel : ViewModelBase, INotifyDataErrorInfo
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

        public NotifyErrorInfoViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            //Doing this will cause the errors to show immediately
            Validate(nameof(FirstName));
            Validate(nameof(LastName));
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

        public override void RaisePropertyChanged(string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            Validate(propertyName);
        }

        private void Validate(string changedPropertyName)
        {
            //Do whatever validation is needed here
            //You can do validation accross multiple properties as well.
            switch (changedPropertyName)
            {
                case nameof(FirstName):
                    if (string.IsNullOrWhiteSpace(FirstName))
                    {
                        _ValidationErrorsByProperty[nameof(FirstName)] = new List<object> { " A first name is required." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FirstName)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(FirstName)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FirstName)));
                    }
                    break;
                case nameof(LastName):
                    if (string.IsNullOrWhiteSpace(LastName))
                    {
                        _ValidationErrorsByProperty[nameof(LastName)] = new List<object> { " A last name is required." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(LastName)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(LastName)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(LastName)));
                    }
                    break;
            }

            //Notify the command that it needs to invoke its CanExecute method (in this case CanSubmit).
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors))
            {
                return errors;
            }
            return Array.Empty<object>();
        }

        private readonly Dictionary<string, List<object>> _ValidationErrorsByProperty =
            new Dictionary<string, List<object>>();

        public bool HasErrors => _ValidationErrorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}