using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;

namespace OldSchoolMdiDemo
{
    public class ClassicMdiChild : ContentControl
    {
        public ClassicMdiChild()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PresentationSource.AddSourceChangedHandler(this, OnPresentationSourceChanged);
            }
        }

        private void OnPresentationSourceChanged(object sender, SourceChangedEventArgs e)
        {
        }
    }
}
