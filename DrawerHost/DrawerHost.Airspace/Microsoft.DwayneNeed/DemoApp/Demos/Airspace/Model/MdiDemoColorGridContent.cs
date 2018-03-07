using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DemoApp.Demos.Airspace.Model
{
    public class MdiDemoColorGridContent : MdiDemoContent
    {
        #region NumElements
        /// <summary>
        ///     The number of cells in the color grid.
        /// </summary>
        public int NumberOfCells
        {
            get { return _numberOfCells; }
            set
            {
                SetValue("NumberOfCells", ref _numberOfCells, value);
            }
        }

        private int _numberOfCells;
        #endregion
    }
}
