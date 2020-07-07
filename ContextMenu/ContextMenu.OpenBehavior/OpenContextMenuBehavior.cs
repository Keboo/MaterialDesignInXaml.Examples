using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ContextMenu.OpenBehavior
{
    public class OpenContextMenuBehavior : Behavior<Control>
    {
        public bool OpenContextMenu
        {
            get => (bool)GetValue(OpenContextMenuProperty);
            set => SetValue(OpenContextMenuProperty, value);
        }

        // Using a DependencyProperty as the backing store for OpenContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenContextMenuProperty =
            DependencyProperty.Register("OpenContextMenu", typeof(bool), 
                typeof(OpenContextMenuBehavior), new PropertyMetadata(false, OnOpenContextMenuChanged));

        private static void OnOpenContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool boolValue && boolValue)
            {
                ((OpenContextMenuBehavior)d).OpenMenu();
            }
        }

        private void OpenMenu()
        {
            if (AssociatedObject.ContextMenu is { } contextMenu)
            {
                contextMenu.IsOpen = true;
            }
        }

    }
}
