using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVM.Async
{
    public class LatestViewModel : ObservableObject
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

        private CancellationTokenSource _Cts;

        private async void StartLongRunningWork()
        {
            try
            {
                var cts = new CancellationTokenSource();
                Interlocked.Exchange(ref _Cts, cts)?.Cancel();
                await DoLongRunningWork(cts.Token);
            }
            catch (OperationCanceledException)
            { }
            catch (Exception e)
            {
                //TODO: log or handle to prevent app crash
            }
        }

        private int _Counter;
        private async Task DoLongRunningWork(CancellationToken token)
        {
            int counter = ++_Counter;
            Data = $"Starting work {counter}";
            await Task.Delay(TimeSpan.FromSeconds(1.5), token);
            Data = $"Completed work {counter}";
        }
    }
}
