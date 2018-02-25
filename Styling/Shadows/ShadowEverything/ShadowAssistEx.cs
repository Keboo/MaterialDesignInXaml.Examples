using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Effects;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;

namespace ShadowEverything
{
    public static class ShadowAssistEx
    {
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.RegisterAttached(
            "ShadowDepth", typeof(ShadowDepth), typeof(ShadowAssistEx), new PropertyMetadata(default(ShadowDepth), OnShadowDepthChanged));

        private static void OnShadowDepthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is UIElement element)
            {
                element.Effect =
                    ShadowConverter.Instance.Convert(e.NewValue, typeof(Effect), null, CultureInfo.CurrentUICulture) as Effect;
            }
        }

        public static void SetShadowDepth(DependencyObject element, ShadowDepth value)
        {
            element.SetValue(ShadowDepthProperty, value);
        }

        public static ShadowDepth GetShadowDepth(DependencyObject element)
        {
            return (ShadowDepth) element.GetValue(ShadowDepthProperty);
        }
    }
}