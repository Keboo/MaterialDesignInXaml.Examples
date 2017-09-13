using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Trans
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private int _PageIndex;
        public int PageIndex
        {
            get => _PageIndex;
            set => Set(ref _PageIndex, value);
        }

        private bool _IsAllPages = true;
        public bool IsAllPages
        {
            get => _IsAllPages;
            set => Set(ref _IsAllPages, value);
        }

        private bool _IsOneAndFour;
        public bool IsOneAndFour
        {
            get => _IsOneAndFour;
            set => Set(ref _IsOneAndFour, value);
        }

        private bool _isOneTwoAndFour;
        public bool IsOneTwoAndFour
        {
            get => _isOneTwoAndFour;
            set => Set(ref _isOneTwoAndFour, value);
        }

        public MainViewModel()
        {
            NextPageCommand = new RelayCommand(OnNext, CanNext);
            PreviousPageCommand = new RelayCommand(OnPrevious, CanPrevious);
        }

        private bool CanPrevious()
        {
            int? previousIndex = GetPreviousIndex();
            return previousIndex != null && previousIndex != PageIndex;
        }

        private void OnPrevious()
        {
            int? previousIndex = GetPreviousIndex();
            if (previousIndex != null)
            {
                PageIndex = previousIndex.Value;
            }
        }

        private bool CanNext()
        {
            int? nextIndex = GetNextIndex();
            return nextIndex != null && nextIndex != PageIndex;
        }

        private void OnNext()
        {
            int? nextIndex = GetNextIndex();
            if (nextIndex != null)
            {
                PageIndex = nextIndex.Value;
            }
        }

        private int? GetNextIndex()
        {
            if (IsAllPages)
            {
                return Math.Min(3, PageIndex + 1);
            }
            if (IsOneAndFour)
            {
                return 3;
            }
            if (IsOneTwoAndFour)
            {
                switch (PageIndex)
                {
                    case 0: return 1;
                    case 1:
                    case 3: return 3;
                }
            }
            return null;
        }

        private int? GetPreviousIndex()
        {
            if (IsAllPages)
            {
                return Math.Max(0, PageIndex - 1);
            }
            if (IsOneAndFour)
            {
                return 0;
            }
            if (IsOneTwoAndFour)
            {
                switch (PageIndex)
                {
                    case 0:
                    case 1: return 0;
                    case 3: return 1;
                }
            }
            return null;
        }
    }
}