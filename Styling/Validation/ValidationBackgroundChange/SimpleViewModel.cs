using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace ValidationBackgroundChange
{
    public class SimpleViewModel : AnnotationValidationViewModel
    {
        private string _FirstName;
        [Required]
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
        
        public SimpleViewModel()
        {
            //This will force validation to show initially
            ValidateModel();
        }
    }
}