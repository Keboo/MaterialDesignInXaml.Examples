using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeViewMenu
{
    public static class TreeViewHelper
    {
        public static readonly DependencyProperty SingleExpandPathProperty = DependencyProperty.RegisterAttached(
            "SingleExpandPath", typeof(bool), typeof(TreeViewHelper), new PropertyMetadata(default(bool), OnSingleExpandPathChanged));

        private static void OnSingleExpandPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as bool? == true)
            {
                ((TreeView)d).SelectedItemChanged += TreeViewOnSelectedItemChanged;
            }
            else
            {
                ((TreeView)d).SelectedItemChanged -= TreeViewOnSelectedItemChanged;
            }
        }

        private static void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var seen = new HashSet<TreeViewItem>();

            var treeView = (TreeView)sender;

            if (GetTreeViewItem(e.NewValue) is TreeViewItem newTvi)
            {
                newTvi.IsExpanded = !newTvi.IsExpanded;
                foreach (var parents in GetParents(newTvi))
                {
                    seen.Add(parents);
                }

                foreach (TreeViewItem tvi in GetExpanded(treeView))
                {
                    if (!seen.Contains(tvi))
                        tvi.IsExpanded = false;
                }
            }
            else if (GetTreeViewItem(e.OldValue) is TreeViewItem oldTvi)
            {
                oldTvi.IsExpanded = !oldTvi.IsExpanded;
            }

            IEnumerable<TreeViewItem> GetExpanded(ItemsControl parent)
            {
                foreach (TreeViewItem tvi in GetTreeViewItems(parent).Where(x => x != null))
                {
                    if (tvi.IsExpanded) 
                    {
                        yield return tvi;

                        foreach (TreeViewItem child in GetExpanded(tvi))
                        {
                            yield return child;
                        }
                    }
                }
            }

            IEnumerable<TreeViewItem> GetTreeViewItems(ItemsControl parent)
            {
                return parent.Items.Cast<object>().Select(x => GetTreeViewItem(x, parent));
            }

            TreeViewItem GetTreeViewItem(object item, ItemsControl parent = null)
            {
                if (item is TreeViewItem tvi) return tvi;
                return (parent ?? treeView).ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
            }

            IEnumerable<TreeViewItem> GetParents(TreeViewItem tvi)
            {
                for (; tvi != null; tvi = tvi.Parent as TreeViewItem)
                    yield return tvi;
            }
        }

        public static void SetSingleExpandPath(DependencyObject element, bool value)
        {
            element.SetValue(SingleExpandPathProperty, value);
        }

        public static bool GetSingleExpandPath(DependencyObject element)
        {
            return (bool)element.GetValue(SingleExpandPathProperty);
        }

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