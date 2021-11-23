using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Utilities
{
    public class Modifier : ModifierBase
    {
        public DependencyProperty Property { get; set; }
        public object Value { get; set; }
        public string TemplatePartName { get; set; }
        public string VisualElementName { get; set; }

        public override void Apply(DependencyObject target)
        {
            if (target is FrameworkElement element && Property is DependencyProperty property)
            {
                if (target.GetValue(Control.TemplateProperty) is ControlTemplate template &&
                    template.FindName(TemplatePartName, element) is DependencyObject templatePart)
                {
                    if (Value?.GetType().IsSubclassOf(property.PropertyType) == true)
                    {
                        templatePart.SetCurrentValue(property, Value);
                    }
                    else
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        templatePart.SetCurrentValue(property, converter.ConvertFrom(Value));
                    }
                }
                else if (element.FindName(TemplatePartName) is DependencyObject childElement)
                {
                    childElement.SetValue(property, Value);
                }
            }
        }
    }
}