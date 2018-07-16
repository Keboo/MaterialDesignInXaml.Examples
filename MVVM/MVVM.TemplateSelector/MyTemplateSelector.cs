using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MVVM.TemplateSelector
{
    [ContentProperty(nameof(Templates))]
    public class MyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Templates { get; set; } = new ResourceDictionary();



        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Option option)
            {
                return Templates[option.Type] as DataTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}