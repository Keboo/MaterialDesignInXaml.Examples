using System.Windows;
using System.Windows.Controls;

namespace Utilities
{
    public class Modifier : ModifierBase
    {
        public DependencyProperty Property { get; set; }
        public object Value { get; set; }
        public string TemplatePartName { get; set; }
        public override void Apply(DependencyObject target)
        {
            if (target is FrameworkElement element &&
                target.GetValue(Control.TemplateProperty) is ControlTemplate template &&
                template.FindName(TemplatePartName, element) is DependencyObject templatePart)
            {
                templatePart.SetCurrentValue(Property, Value);
            }
        }
    }
}