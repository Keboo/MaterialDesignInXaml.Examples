using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVM.Async
{
    public class DebounceViewModel : ObservableObject
    {
        private bool _IsChecked;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                if (SetProperty(ref _IsChecked, value))
                {
                    //Do Async work
                    StartLongRunningWork();
                }
            }
        }

        private string _Data;
        public string Data
        {
            get => _Data;
            set => SetProperty(ref _Data, value);
        }

        private int _IsRunning;
        private async void StartLongRunningWork()
        {
            try
            {
                //Atomic operation to both check if we are already running and set the flag if we are not
                if (Interlocked.CompareExchange(ref _IsRunning, 1, 0) == 0)
                {
                    await DoLongRunningWork();
                    Interlocked.Exchange(ref _IsRunning, 0);
                }
            }
            catch (Exception e)
            {
                //TODO: log or handle to prevent app crash
            }
        }

        private int _Counter;
        private async Task DoLongRunningWork()
        {
            int counter = ++_Counter;
            Data = $"Starting work {counter}";
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            Data = $"Completed work {counter}";
        }
    }
}
