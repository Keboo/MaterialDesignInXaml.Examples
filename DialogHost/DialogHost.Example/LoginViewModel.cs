using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DialogHost.Example
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler Close;
        public ICommand LoginCommand { get; }

        private string _Username;
        public string Username
        {
            get => _Username;
            set => Set(ref _Username, value);
        }

        //WARNING: Very bad security! Demo only! DO NOT DO THIS!
        private string _Password;
        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        private bool _IsValidating;
        public bool IsValidating
        {
            get => _IsValidating;
            set => Set(ref _IsValidating, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private async void OnLogin()
        {
            bool result = await ValidateLogin(Username, Password);
            if (result)
            {
                Close?.Invoke(this, EventArgs.Empty);
            }
            //TODO: Else show error indicating failed login
        }

        private async Task<bool> ValidateLogin(string username, string password)
        {
            try
            {
                IsValidating = true;
                //TODO: actual validation
                await Task.Delay(TimeSpan.FromSeconds(2));

                return username == "admin" && password == "password";
            }
            finally
            {
                IsValidating = false;
            }
        }
    }
}