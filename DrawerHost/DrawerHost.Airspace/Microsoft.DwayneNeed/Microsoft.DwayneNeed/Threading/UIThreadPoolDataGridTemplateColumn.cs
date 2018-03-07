using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Microsoft.DwayneNeed.Threading
{
    public class UIThreadPoolDataGridTemplateColumn : DataGridTemplateColumn
    {
        public string PropertyName {get;set;}

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            // Get the cell template and make sure it is sealed because we need
            // to inflate it on the worker thread.
            DataTemplate cellTemplate = CellTemplate;
            if (!cellTemplate.IsSealed)
            {
                cellTemplate.Seal();
            }

            UIThreadPoolRoot root = new UIThreadPoolRoot();
            root.DataContext = dataItem;
            root.Content = cellTemplate;
            root.PropertyName = PropertyName;

            return root;
        }
    }
}
