using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace MultiThreadDataGridDemo
{
    // A silly "data" class that contains a variety of properties of various
    // types to demonstrate data binding scenarios.  All of the property values
    // are generated randomly.
    public class DataItem : INotifyPropertyChanged
    {
        public DataItem(int id)
        {
            _id = id;
            _random = new Random(_id);
            Update();
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("ID"));
                    }
                }
            }
        }

        private NotificationConfiguration _configuration;
        public NotificationConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                if (value != _configuration)
                {
                    _configuration = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Configuration"));
                    }
                }
            }
        }

        private int _a;
        public int A
        {
            get { return _a; }
            private set
            {
                if (value != _a)
                {
                    _a = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.A)
                    {
                        handler(this, new PropertyChangedEventArgs("A"));
                    }
                }
            }
        }

        private string _b;
        public string B
        {
            get { return _b; }
            private set
            {
                if (value != _b)
                {
                    _b = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.B)
                    {
                        handler(this, new PropertyChangedEventArgs("B"));
                    }
                }
            }
        }

        private float _c;
        public float C
        {
            get { return _c; }
            private set
            {
                if (value != _c)
                {
                    _c = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.C)
                    {
                        handler(this, new PropertyChangedEventArgs("C"));
                    }
                }
            }
        }

        private long _d;
        public long D
        {
            get { return _d; }
            private set
            {
                if (value != _d)
                {
                    _d = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.D)
                    {
                        handler(this, new PropertyChangedEventArgs("D"));
                    }
                }
            }
        }

        private char _e;
        public char E
        {
            get { return _e; }
            private set
            {
                if (value != _e)
                {
                    _e = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.E)
                    {
                        handler(this, new PropertyChangedEventArgs("E"));
                    }
                }
            }
        }

        private Color _f;
        public Color F
        {
            get { return _f; }
            private set
            {
                if (value != _f)
                {
                    _f = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.F)
                    {
                        handler(this, new PropertyChangedEventArgs("F"));
                    }
                }
            }
        }

        private Brush _g;
        public Brush G
        {
            get { return _g; }
            private set
            {
                if (value != _g)
                {
                    _g = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.G)
                    {
                        handler(this, new PropertyChangedEventArgs("G"));
                    }
                }
            }
        }

        private Decimal _h;
        public Decimal H
        {
            get { return _h; }
            private set
            {
                if (value != _h)
                {
                    _h = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.H)
                    {
                        handler(this, new PropertyChangedEventArgs("H"));
                    }
                }
            }
        }

        private double _i;
        public double I
        {
            get { return _i; }
            private set
            {
                if (value != _i)
                {
                    _i = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.I)
                    {
                        handler(this, new PropertyChangedEventArgs("I"));
                    }
                }
            }
        }

        private SomeEnum _j;
        public SomeEnum J
        {
            get { return _j; }
            private set
            {
                if (value != _j)
                {
                    _j = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null && _configuration != null && _configuration.J)
                    {
                        handler(this, new PropertyChangedEventArgs("J"));
                    }
                }
            }
        }
 
        public void Update()
        {
            A = _random.Next(100);
            B = _words[_random.Next(100)];
            C = (float) (_random.NextDouble() * 100.0);
            D++;
            E = (char)((int)'A' + _random.Next(26));
            F = Color.FromScRgb(1.0f, (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());

            var brush = new SolidColorBrush(_f);
            brush.Freeze();
            G = brush;

            H = new Decimal(_random.NextDouble() * 100.0);
            I = _random.NextDouble() * 100.0;
            J = (SomeEnum)_random.Next(6);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Random _random;
        
        // The top 100 words (from tv scripts) in alphabetical order:
        private string[] _words = { "a", "about", "all", "and", "are", "as", "at", "back", "be", "because", "been", "but", "can", "can't", "come", "could", "did", "didn't", "do", "don't", "for", "from", "get", "go", "going", "good", "got", "had", "have", "he", "her", "here", "he's", "hey", "him", "his", "how", "I", "if", "I'll", "I'm", "in", "is", "it", "it's", "just", "know", "like", "look", "me", "mean", "my", "no", "not", "now", "of", "oh", "OK", "okay", "on", "one", "or", "out", "really", "right", "say", "see", "she", "so", "some", "something", "tell", "that", "that's", "the", "then", "there", "they", "think", "this", "time", "to", "up", "want", "was", "we", "well", "were", "what", "when", "who", "why", "will", "with", "would", "yeah", "yes", "you", "your", "you're" };
    }
}
