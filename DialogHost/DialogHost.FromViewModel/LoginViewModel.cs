namespace DialogHost.FromViewModel
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        //TODO: BAD BAD BAD, Use SecureString!
        public string Password { get; set; }
    }
}