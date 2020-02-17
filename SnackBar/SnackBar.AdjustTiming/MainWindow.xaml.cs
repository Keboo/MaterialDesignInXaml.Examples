using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.Windows;

namespace Snackbar.AdjustTiming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SnackbarMessageQueue Queue { get; } = new SnackbarMessageQueue();

        public MainWindow()
        {
            InitializeComponent();

            //You can always take full control of the snackbar and manually set/clear the Message property rather than using a SnackbarMessageQueue
            //For this example we will use the queue
            Snackbar.MessageQueue = Queue;
        }

        private void ShowSnackbar_Click(object sender, RoutedEventArgs e)
        {
            string content;
            TimeSpan? durationOverride = null;
            if (!string.IsNullOrWhiteSpace(TimingTextBox.Text) &&
                TimeSpan.TryParse(TimingTextBox.Text, out TimeSpan messageDisplayTime) &&
                messageDisplayTime > TimeSpan.Zero)
            {
                durationOverride = messageDisplayTime;
                content = $"Duration override {messageDisplayTime}";
            }
            else
            {
                content = "No duration override";
            }
            //Most of these parameters are optional and will accept null
            Queue.Enqueue(content, "Action", _ => Debug.WriteLine("Action Invoked!"), null, false, true, durationOverride);
        }
    }
}
