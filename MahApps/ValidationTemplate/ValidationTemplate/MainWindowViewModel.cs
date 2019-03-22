using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ValidationTemplate.Annotations;

namespace ValidationTemplate
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _NotEmpty;
        public string NotEmpty
        {
            get => _NotEmpty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Can't be empty");
                }
                if (_NotEmpty != value)
                {
                    _NotEmpty = value;
                    OnPropertyChanged();
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}