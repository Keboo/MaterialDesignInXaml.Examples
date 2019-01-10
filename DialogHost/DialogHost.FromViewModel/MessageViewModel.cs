namespace DialogHost.FromViewModel
{
    public class MessageViewModel
    {
        public string Message { get; }

        public MessageViewModel(string message)
        {
            Message = message;
        }
    }
}