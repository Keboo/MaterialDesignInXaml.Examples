using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.DwayneNeed.Media
{
    class DynamicThickness : DependencyObject
    {
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(DynamicThickness), new UIPropertyMetadata(0.0, OnPropertyChanged));

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(DynamicThickness), new UIPropertyMetadata(0.0, OnPropertyChanged));
        
        public static readonly DependencyProperty RightProperty =
            DependencyProperty.Register("Right", typeof(double), typeof(DynamicThickness), new UIPropertyMetadata(0.0, OnPropertyChanged));
        
        public static readonly DependencyProperty BottomProperty =
            DependencyProperty.Register("Bottom", typeof(double), typeof(DynamicThickness), new UIPropertyMetadata(0.0, OnPropertyChanged));

        private static readonly DependencyPropertyKey ValuePropertyKey =
            DependencyProperty.RegisterReadOnly("Value", typeof(Thickness), typeof(DynamicThickness), new UIPropertyMetadata(new Thickness(), OnPropertyChanged));

        public static readonly DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        public double Right
        {
            get { return (double)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        public double Bottom
        {
            get { return (double)GetValue(BottomProperty); }
            set { SetValue(BottomProperty, value); }
        }

        public Thickness Value
        {
            get { return (Thickness)GetValue(BottomProperty); }
            private set { SetValue(ValuePropertyKey, value); }
        }

        private static void OnPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DynamicThickness dt = (DynamicThickness) sender;
            dt.Value = new Thickness(dt.Left, dt.Top, dt.Right, dt.Bottom);
        }
    }
}
