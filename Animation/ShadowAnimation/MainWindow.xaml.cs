using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace ShadowAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var card = new Card
            {
                Margin = new Thickness(10),
                Padding = new Thickness(30),
                Content = new Grid
                {
                    Background = new SolidColorBrush(Colors.White),
                    Children =
                    {
                        new TextBlock {Text = "Done in Code Behind", HorizontalAlignment = HorizontalAlignment.Center}
                    }
                }
            };
            RenderOptions.SetClearTypeHint((Grid)card.Content, ClearTypeHint.Enabled);

            //Turn off the shadow (it defaults to Depth2) - we will do our own so we can animate it
            ShadowAssist.SetShadowDepth(card, ShadowDepth.Depth0);

            var shadow1 = (DropShadowEffect)FindResource("MaterialDesignShadowDepth1");
            var shadow2 = (DropShadowEffect)FindResource("MaterialDesignShadowDepth4");

            //Cloning so we only change this instance
            var effect = new DropShadowEffect
            {
                BlurRadius = shadow1.BlurRadius,
                ShadowDepth = shadow1.ShadowDepth,
                Direction = shadow1.Direction,
                Color = shadow1.Color,
                Opacity = shadow1.Opacity,
                RenderingBias = shadow1.RenderingBias
            };
            card.Effect = effect;

            //Create storyboard for mouse enter
            Storyboard mouseEnterStoryboard = new Storyboard();

            var enterBlurRadiusAnimation = new DoubleAnimation(shadow2.BlurRadius, new Duration(TimeSpan.FromSeconds(0.5)));
            Storyboard.SetTargetProperty(enterBlurRadiusAnimation, new PropertyPath(nameof(DropShadowEffect.BlurRadius)));
            Storyboard.SetTarget(enterBlurRadiusAnimation, effect);
            mouseEnterStoryboard.Children.Add(enterBlurRadiusAnimation);

            var enterShadowDepthAnimation = new DoubleAnimation(shadow2.ShadowDepth, new Duration(TimeSpan.FromSeconds(0.5)));
            Storyboard.SetTargetProperty(enterShadowDepthAnimation, new PropertyPath(nameof(DropShadowEffect.ShadowDepth)));
            Storyboard.SetTarget(enterShadowDepthAnimation, effect);
            mouseEnterStoryboard.Children.Add(enterShadowDepthAnimation);
            
            mouseEnterStoryboard.Freeze();

            //Create storybaord for mouse leave
            Storyboard mouseLeaveStoryboard = new Storyboard();

            var leaveBlurRadiusAnimation = new DoubleAnimation(shadow1.BlurRadius, new Duration(TimeSpan.FromSeconds(0.5)));
            Storyboard.SetTargetProperty(leaveBlurRadiusAnimation, new PropertyPath(nameof(DropShadowEffect.BlurRadius)));
            Storyboard.SetTarget(leaveBlurRadiusAnimation, effect);
            mouseLeaveStoryboard.Children.Add(leaveBlurRadiusAnimation);

            var leaveShadowDepthAnimation = new DoubleAnimation(shadow1.ShadowDepth, new Duration(TimeSpan.FromSeconds(0.5)));
            Storyboard.SetTargetProperty(leaveShadowDepthAnimation, new PropertyPath(nameof(DropShadowEffect.ShadowDepth)));
            Storyboard.SetTarget(leaveShadowDepthAnimation, effect);
            mouseLeaveStoryboard.Children.Add(leaveShadowDepthAnimation);

            mouseLeaveStoryboard.Freeze();


            card.MouseEnter += (sender, e) =>
            {
                card.BeginStoryboard(mouseEnterStoryboard);
            };
            card.MouseLeave += (sender, e) =>
            {
                card.BeginStoryboard(mouseLeaveStoryboard);
            };

            StackPanel.Children.Add(card);
        }
    }
}
