using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MultiThreadDataGridDemo
{
    public class NotificationConfiguration : INotifyPropertyChanged
    {
        private bool _a = true;
        public bool A
        {
            get { return _a; }
            set
            {
                if (value != _a)
                {
                    _a = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("A"));
                    }
                }
            }
        }

        private bool _b = true;
        public bool B
        {
            get { return _b; }
            set
            {
                if (value != _b)
                {
                    _b = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("B"));
                    }
                }
            }
        }

        private bool _c = true;
        public bool C
        {
            get { return _c; }
            set
            {
                if (value != _c)
                {
                    _c = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("C"));
                    }
                }
            }
        }

        private bool _d = true;
        public bool D
        {
            get { return _d; }
            set
            {
                if (value != _d)
                {
                    _d = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("D"));
                    }
                }
            }
        }

        private bool _e = true;
        public bool E
        {
            get { return _e; }
            set
            {
                if (value != _e)
                {
                    _e = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("E"));
                    }
                }
            }
        }

        private bool _f = true;
        public bool F
        {
            get { return _f; }
            set
            {
                if (value != _f)
                {
                    _f = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("F"));
                    }
                }
            }
        }

        private bool _g = true;
        public bool G
        {
            get { return _g; }
            set
            {
                if (value != _g)
                {
                    _g = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("G"));
                    }
                }
            }
        }

        private bool _h = true;
        public bool H
        {
            get { return _h; }
            set
            {
                if (value != _h)
                {
                    _h = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("H"));
                    }
                }
            }
        }

        private bool _i = true;
        public bool I
        {
            get { return _i; }
            set
            {
                if (value != _i)
                {
                    _i = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("I"));
                    }
                }
            }
        }

        private bool _j = true;
        public bool J
        {
            get { return _j; }
            set
            {
                if (value != _j)
                {
                    _j = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("J"));
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
