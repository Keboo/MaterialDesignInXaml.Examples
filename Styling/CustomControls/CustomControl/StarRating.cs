using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControl
{
    [TemplatePart(Name = ContentWrapper, Type = typeof(Grid))]
    [TemplateVisualState(GroupName = "RatingStates", Name = ScaryRedStateName)]
    [TemplateVisualState(GroupName = "RatingStates", Name = NormalStateName)]
    [TemplateVisualState(GroupName = "RatingStates", Name = HappyGreenStateName)]
    public class StarRating : Control
    {
        public const string ContentWrapper = "PART_Content";

        public const string ScaryRedStateName = "ScaryRed";
        public const string NormalStateName = "Normal";
        public const string HappyGreenStateName = "HappyGreen";

        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(nameof(Value), typeof(int), typeof(StarRating),
                new PropertyMetadata(default(int), OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (StarRating)obj;

            if (e.NewValue is int newValue)
            {
                if (newValue >= 4)
                {
                    VisualStateManager.GoToState(control, ScaryRedStateName, true);
                }
                else if(newValue <= 2)
                {
                    VisualStateManager.GoToState(control, HappyGreenStateName, true);
                }
                else
                {
                    VisualStateManager.GoToState(control, NormalStateName, true);
                }
            }
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public StarRating()
        {
            MouseDoubleClick += StarRating_MouseDoubleClick;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentWrapperGrid = (Grid)GetTemplateChild(ContentWrapper);
            ContentWrapperGrid.RenderTransform = Scale = new ScaleTransform(1,1);
            
            VisualStateManager.GoToState(this, NormalStateName, false);
        }

        private Grid ContentWrapperGrid { get; set; }
        private ScaleTransform Scale { get; set; }
        private bool IsBig { get; set; }

        private void StarRating_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsBig = !IsBig;
            
            if (IsBig)
            {
                Scale.ScaleX = 1.2;
                Scale.ScaleY = 1.2;
            }
            else
            {
                Scale.ScaleX = 1;
                Scale.ScaleY = 1;
            }
        }
    }
}
