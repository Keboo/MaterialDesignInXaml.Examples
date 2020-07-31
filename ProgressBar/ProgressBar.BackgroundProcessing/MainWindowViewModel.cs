using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;

namespace ProgressBar.BackgroundProcessing
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand RetrieveDataCommand { get; }

        private bool _IsRunning;
        private bool IsRunning
        {
            get => _IsRunning;
            set
            {
                _IsRunning = value;
                RetrieveDataCommand.RaiseCanExecuteChanged();
            }
        }

        private int _Progress;
        public int Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
        }

        public MainWindowViewModel()
        {
            RetrieveDataCommand = new RelayCommand(OnRetrieveData, () => !IsRunning);
        }

        private async void OnRetrieveData()
        {
            IsRunning = true;
            //Invoked on the UI thread
            Progress = 0;
            //Run RetrieveDataAsync on a background thread
            await Task.Run(RetrieveDataAsync);
            //Also invoked on the UI thread
            Progress = 100;
            IsRunning = false;
        }

        private async Task RetrieveDataAsync()
        {
            var random = new Random();
            do
            {
                //This is just a simple delay. Replace with any long running operations
                //Such and File IO, Database Queries, or Network calls.
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                Progress = Math.Min(99, Progress + random.Next(10));
            }
            while (Progress < 99);
        }
    }
}
