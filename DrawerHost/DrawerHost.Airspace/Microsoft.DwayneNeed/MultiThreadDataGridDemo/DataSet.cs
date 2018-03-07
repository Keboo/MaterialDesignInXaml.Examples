using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;

namespace MultiThreadDataGridDemo
{
    class DataSet : INotifyPropertyChanged
    {
        public DataSet()
        {
            AutoResetEvent isRunning = new AutoResetEvent(false);

            // Spin up our worker thread.  The important aspects of this
            // thread are:
            // 1) It runs a WPF Dispatcher loop, which is a message pump.
            // 2) It is marked as STA.
            // 3) It is marked as a background thread, which will allow the
            //    app to exit even with active UIThreadPool threads.
            _thread = new Thread(new ParameterizedThreadStart(Run));

            _thread.Name = "DataSet Thread";
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start(isRunning);

            // Wait for the worker thread to get started.
            isRunning.WaitOne();
            isRunning.Dispose();

            Dispatcher workerDispatcher = Dispatcher.FromThread(_thread);
            workerDispatcher.Invoke((Action) delegate
            {
                Items = new DataItem[0];

                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(1 / 30.0);
                _timer.Tick += UpdateDataItems;
                _timer.IsEnabled = UpdateItems;
            });
        }

        public int Count
        {
            get
            {
                return Items.Length;
            }

            set
            {
                int count = value >= 0 ? value : 0;

                Dispatcher workerDispatcher = Dispatcher.FromThread(_thread);
                workerDispatcher.Invoke((Action)delegate
                {
                    if (count != Items.Length)
                    {
                        DataItem[] newItems = new DataItem[count];
                        int oldCount = Math.Min(Items.Length, count);
                        int i = 0;

                        for (; i < oldCount; i++)
                        {
                            newItems[i] = Items[i];
                        }

                        for (; i < count; i++)
                        {
                            newItems[i] = new DataItem(i);
                            newItems[i].Configuration = Configuration;
                        }

                        Items = newItems;

                        PropertyChangedEventHandler handler = PropertyChanged;
                        if (handler != null)
                        {
                            handler(this, new PropertyChangedEventArgs("Count"));
                        }
                    }
                });
            }
        }

        private NotificationConfiguration _configuration;
        public NotificationConfiguration Configuration
        {
            get
            {
                return _configuration;
            }

            set
            {
                Dispatcher workerDispatcher = Dispatcher.FromThread(_thread);
                workerDispatcher.Invoke((Action)delegate
                {
                    _configuration = value;

                    foreach (DataItem dataItem in Items)
                    {
                        dataItem.Configuration = _configuration;
                    }

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Configuration"));
                    }
                });
            }
        }

        private bool _updateItems;
        public bool UpdateItems
        {
            get { return _updateItems; }
            set
            {
                Dispatcher workerDispatcher = Dispatcher.FromThread(_thread);
                workerDispatcher.Invoke((Action)delegate
                {
                    if (value != _updateItems)
                    {
                        _updateItems = value;
                        _timer.IsEnabled = value;

                        PropertyChangedEventHandler handler = PropertyChanged;
                        if (handler != null)
                        {
                            handler(this, new PropertyChangedEventArgs("UpdateItems"));
                        }
                    }
                });
            }
        }

        private DataItem[] _items;
        public DataItem[] Items
        {
            get { return _items; }
            private set
            {
                if (value != _items)
                {
                    _items = value;

                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("Items"));
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateDataItems(object sender, EventArgs e)
        {
            foreach (DataItem dataItem in Items)
            {
                dataItem.Update();
            }
        }

        private static void Run(object parameter)
        {
            // Force the creation of the Dispatcher for this thread, and then
            // signal that we are running.
            Dispatcher d = Dispatcher.CurrentDispatcher;
            ((AutoResetEvent)parameter).Set();

            // Run a dispatcher for this UIThreadPool thread.
            // This is the central processing loop for WPF.
            Dispatcher.Run();
        }
        
        private Thread _thread;
        private DispatcherTimer _timer;
        private Random _random = new Random();
        private TimeSpan _lastRenderingTime = TimeSpan.MinValue;
    }
}
