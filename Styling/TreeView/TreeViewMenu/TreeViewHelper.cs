using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeViewMenu
{
    public static class TreeViewHelper
    {
        public static readonly DependencyProperty SelectedCommandProperty = DependencyProperty.RegisterAttached(
            "SelectedCommand", typeof(ICommand), typeof(TreeViewHelper), new PropertyMetadata(default(ICommand), OnSelectedCommandChanged));

        private static void OnSelectedCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TreeViewItem treeViewItem)
            {
                if (e.NewValue != null)
                {
                    treeViewItem.Selected += TreeViewItemOnSelected;
                }
                else
                {
                    treeViewItem.Selected -= TreeViewItemOnSelected;
                }
            }
            else
            {
                throw new InvalidOperationException($"{nameof(TreeViewHelper)}.{SelectedCommandProperty.Name} can only be used with {nameof(TreeViewItem)}s");
            }
        }

        private static void TreeViewItemOnSelected(object sender, RoutedEventArgs routedEventArgs)
        {
            var tvi = (TreeViewItem)sender;
            ICommand command = GetSelectedCommand(tvi);
            object parameter = GetSelectedCommandParameter(tvi);
            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }

        public static void SetSelectedCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(SelectedCommandProperty, value);
        }

        public static ICommand GetSelectedCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(SelectedCommandProperty);
        }

        public static readonly DependencyProperty SelectedCommandParameterProperty = DependencyProperty.RegisterAttached(
            "SelectedCommandParameter", typeof(object), typeof(TreeViewHelper), new PropertyMetadata(default(object)));

        public static void SetSelectedCommandParameter(DependencyObject element, object value)
        {
            element.SetValue(SelectedCommandParameterProperty, value);
        }

        public static object GetSelectedCommandParameter(DependencyObject element)
        {
            return element.GetValue(SelectedCommandParameterProperty);
        }
    }
}