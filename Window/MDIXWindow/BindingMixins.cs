using System;
using System.Windows;
using System.Windows.Data;

namespace MDIXWindow
{
    public static class BindingMixins
    { 
        private static readonly DependencyProperty EvalProperty = DependencyProperty.RegisterAttached(
            "Eval", typeof(object), typeof(BindingMixins), new PropertyMetadata(default(object)));

        public static T Evaluate<T>(this BindingBase binding, DependencyObject target)
        {
            if (binding == null) throw new ArgumentNullException(nameof(binding));
            if (target == null) throw new ArgumentNullException(nameof(target));

            BindingOperations.SetBinding(target, EvalProperty, binding);
            if (target.GetValue(EvalProperty) is T value)
            {
                return value;
            }
            return default(T);
        }
    }
}