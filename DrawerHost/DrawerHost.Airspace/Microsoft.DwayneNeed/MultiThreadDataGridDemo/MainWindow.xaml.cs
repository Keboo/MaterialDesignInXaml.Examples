using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.DwayneNeed.Threading;
using System.Windows.Threading;
using System.Collections;

namespace MultiThreadDataGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _configuration = Resources["Configuration"] as NotificationConfiguration;
            TheDataGrid.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(TheDataGrid_AutoGeneratingColumn);
        }

        void TheDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (UseUIThreadPool.IsChecked.GetValueOrDefault())
            {
                var column = new UIThreadPoolDataGridTemplateColumn();
                if (e.Column != null)
                {
                    // Copy most of the properties from the default column.
                    column.ClipboardContentBinding = e.Column.ClipboardContentBinding;
                    column.CanUserReorder = e.Column.CanUserReorder;
                    column.CanUserResize = e.Column.CanUserResize;
                    column.CanUserSort = e.Column.CanUserSort;
                    if (e.Column.DisplayIndex >= 0)
                    {
                        column.DisplayIndex = e.Column.DisplayIndex;
                    }
                    column.DragIndicatorStyle = e.Column.DragIndicatorStyle;
                    column.Header = e.Column.Header;
                    column.HeaderStringFormat = e.Column.HeaderStringFormat;
                    column.HeaderStyle = e.Column.HeaderStyle;
                    column.HeaderTemplate = e.Column.HeaderTemplate;
                    column.HeaderTemplateSelector = e.Column.HeaderTemplateSelector;
                    column.Width = e.Column.Width;
                    column.MinWidth = e.Column.MinWidth;
                    column.MaxWidth = e.Column.MaxWidth;
                    column.SortDirection = e.Column.SortDirection;
                    column.SortMemberPath = e.Column.SortMemberPath;
                    column.Visibility = e.Column.Visibility;

                    // Replace the cell template with our own!
                    column.CellTemplate = Resources["UIThreadPoolRootTemplate"] as DataTemplate;
                    column.PropertyName = e.PropertyName;
                    e.Column = column;
                }
            }
            else
            {
                // let the system generate its own columns on the UI thread.
            }
        }

        private void ConfigurationAll_Click(object sender, RoutedEventArgs e)
        {
            _configuration.A = true;
            _configuration.B = true;
            _configuration.C = true;
            _configuration.D = true;
            _configuration.E = true;
            _configuration.F = true;
            _configuration.G = true;
            _configuration.H = true;
            _configuration.I = true;
            _configuration.J = true;
        }

        private void ConfigurationNone_Click(object sender, RoutedEventArgs e)
        {
            _configuration.A = false;
            _configuration.B = false;
            _configuration.C = false;
            _configuration.D = false;
            _configuration.E = false;
            _configuration.F = false;
            _configuration.G = false;
            _configuration.H = false;
            _configuration.I = false;
            _configuration.J = false;
        }

        private NotificationConfiguration _configuration;

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            // Disconnect the items source and reconnect it.  This will
            // regenerate the rows.
            IEnumerable itemsSource = TheDataGrid.ItemsSource;
            TheDataGrid.ItemsSource = null;
            TheDataGrid.ItemsSource = itemsSource;
        }
    }
}
