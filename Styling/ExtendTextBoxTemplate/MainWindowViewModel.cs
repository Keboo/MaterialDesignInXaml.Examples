using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExtendTextBoxTemplate
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string _Text;
        public string Text
        {
            get => _Text;
            set
            {
                if (_Text != value)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new Exception("Must enter a value");
                    }
                    _Text = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
