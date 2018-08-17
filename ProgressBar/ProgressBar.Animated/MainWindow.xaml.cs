using System;
using System.Windows;

namespace ProgressBar.Animated
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PreviousOnClick(object sender, RoutedEventArgs e)
        {
            if (AnimateBehavior.IsAnimating) return;
            ProgressBar.Value = Math.Max(ProgressBar.Minimum, ProgressBar.Value - 25);
        }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {
            if (AnimateBehavior.IsAnimating) return;
            ProgressBar.Value = Math.Min(ProgressBar.Maximum, ProgressBar.Value + 25);
        }
    }
}
