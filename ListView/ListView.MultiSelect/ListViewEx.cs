using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ListView.MultiSelect;

public class ListViewEx : Behavior<System.Windows.Controls.ListView>
{
    public ListViewEx()
    {
        
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += ListView_SelectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= ListView_SelectionChanged;
        base.OnDetaching();
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        System.Windows.Controls.ListView lv = (System.Windows.Controls.ListView)sender;
        object[] selectedItems = [.. lv.SelectedItems];

        //NB: Unregister to avoid stack overflow
        AssociatedObject.SelectionChanged -= ListView_SelectionChanged;
        SetSelectedItems(this, selectedItems);
        AssociatedObject.SelectionChanged += ListView_SelectionChanged;
    }

    public static IList? GetSelectedItems(DependencyObject obj)
    {
        return (IList?)obj.GetValue(SelectedItemsProperty);
    }

    public static void SetSelectedItems(DependencyObject obj, IList? value)
    {
        obj.SetValue(SelectedItemsProperty, value);
    }

    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ListViewEx), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (ListViewEx)d;

        behavior.AssociatedObject.SelectedItems.Clear();
        foreach(var newItem in e.NewValue as IList ?? Array.Empty<object>())
        {
            behavior.AssociatedObject.SelectedItems.Add(newItem);
        }
    }
}