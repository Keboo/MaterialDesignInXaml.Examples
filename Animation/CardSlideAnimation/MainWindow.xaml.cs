using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CardSlideAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Storyboard _storyboard;
        public MainWindow()
        {
            InitializeComponent();

            Storyboard sb = new Storyboard();
            sb.FillBehavior = FillBehavior.HoldEnd;

            var opacityAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(nameof(Card.Opacity)));

            sb.Children.Add(opacityAnimation);

            var translateAnimation = new DoubleAnimation(80, 0, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTargetProperty(translateAnimation, new PropertyPath($"{nameof(Card.RenderTransform)}.{nameof(TranslateTransform.X)}"));

            sb.Children.Add(translateAnimation);

            _storyboard = sb;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _storyboard.Begin(Card);
        }
    }
}
