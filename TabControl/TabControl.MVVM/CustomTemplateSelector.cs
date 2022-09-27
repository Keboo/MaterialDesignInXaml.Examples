using System;
using System.Windows;
using System.Windows.Controls;

namespace TabControl.MVVM;

public class CustomTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Template1 { get; set; }
    public DataTemplate? Template2 { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            Item1ViewModel => Template1,
            Item2ViewModel => Template2,
            _ => throw new InvalidOperationException("Unknown item type"),
        };
    }
}
