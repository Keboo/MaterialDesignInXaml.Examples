using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVM.Async
{
    public class BlockingViewModel : ObservableObject
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

        private async void StartLongRunningWork()
        {
            try
            {
                await DoLongRunningWork();
            }
            catch (Exception e)
            {
                //TODO: log or handle to prevent app crash
            }
        }

        private SemaphoreSlim _SyncLock = new(1, 1);
        private int _Counter;
        private async Task DoLongRunningWork()
        {
            await _SyncLock.WaitAsync();
            try
            {
                int counter = ++_Counter;
                Data = $"Starting work {counter}";
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                Data = $"Completed work {counter}";
            }
            finally
            {
                _SyncLock.Release();
            }
        }
    }
}
