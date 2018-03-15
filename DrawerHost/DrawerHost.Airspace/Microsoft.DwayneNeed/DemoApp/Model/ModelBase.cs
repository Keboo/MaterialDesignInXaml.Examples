using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DemoApp.Model
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetValue<T>(string name, ref T storage, T value)
        {
            // Use the dynamic keyword to enable the use of operators like "!=".
            dynamic dStorage = storage;

            if (dStorage != value)
            {
                storage = value;
                RaisePropertyChanged(name);

                return true;
            }

            return false;
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
