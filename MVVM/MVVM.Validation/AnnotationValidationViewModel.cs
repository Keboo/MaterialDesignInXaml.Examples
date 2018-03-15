using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;

namespace MVVM.Validation
{
    //A simple class that uses the DataAnnotations for validation
    //You will need to add a reference to System.ComponentModel.DataAnnotations for this class to work
    public abstract class AnnotationValidationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, PropertyInfo> _Properties;
        private readonly Dictionary<string, List<object>> _ValidationErrorsByProperty =
            new Dictionary<string, List<object>>();

        protected AnnotationValidationViewModel()
        {
            _Properties = GetType().GetProperties().ToDictionary(x => x.Name);
        }

        public override void RaisePropertyChanged(string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            ValidateProperty(propertyName);
        }

        public bool ValidateModel()
        {
            bool rv = true;
            foreach (string propertyName in _Properties.Keys)
            {
                rv &= ValidateProperty(propertyName);
            }
            return rv;
        }

        public bool ValidateProperty(string propertyName)
        {
            if (_Properties.TryGetValue(propertyName, out PropertyInfo propInfo))
            {
                var errors = new List<object>();
                foreach (var attribute in propInfo.GetCustomAttributes<ValidationAttribute>())
                {
                    if (!attribute.IsValid(propInfo.GetValue(this)))
                    {
                        errors.Add(attribute.FormatErrorMessage(propertyName));
                    }
                }

                if (errors.Any())
                {
                    _ValidationErrorsByProperty[propertyName] = errors;
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                    return false;
                }
                if (_ValidationErrorsByProperty.Remove(propertyName))
                {
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }

            return true;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors))
            {
                return errors;
            }
            return Array.Empty<object>();
        }

        public bool HasErrors => _ValidationErrorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}