using System;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace SnackBarExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var queue = new SnackbarMessageQueue(TimeSpan.FromMinutes(3));
            WelcomeMessageSnackbar.MessageQueue = queue;
            queue.Enqueue("WELCOME");
        }
    }
}
