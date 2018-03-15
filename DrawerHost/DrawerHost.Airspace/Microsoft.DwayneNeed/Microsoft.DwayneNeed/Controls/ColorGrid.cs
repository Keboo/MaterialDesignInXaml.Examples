using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Microsoft.DwayneNeed.Controls
{
    /// <summary>
    ///     A very simply element that displays a grid of colored
    ///     cells.  The only input is the number of cells.  The
    ///     colors are randomly generated.
    /// </summary>
    public class ColorGrid : FrameworkElement
    {
        public static DependencyProperty NumberOfCellsProperty = DependencyProperty.Register(
            /* Name:                 */ "NumberOfCells",
            /* Value Type:           */ typeof(int),
            /* Owner Type:           */ typeof(ColorGrid),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ 1,
            /*     Flags:            */ FrameworkPropertyMetadataOptions.AffectsMeasure,
            /*     Property Changed: */ (d, e) => ((ColorGrid)d).NumberOfCells_PropertyChanged(e)));

        public int NumberOfCells
        {
            get { return (int)GetValue(NumberOfCellsProperty); }
            set { SetValue(NumberOfCellsProperty, value); }
        }

        private void NumberOfCells_PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_uniformGrid != null)
            {
                base.RemoveVisualChild(_uniformGrid);
            }

            int newValue = (int)e.NewValue;
            _uniformGrid = BuildColorGrid(newValue);

            if (_uniformGrid != null)
            {
                AddVisualChild(_uniformGrid);
            }
        }

        private static UniformGrid BuildColorGrid(int numberOfCells)
        {
            Random r = new Random();

            UniformGrid grid = new UniformGrid();
            for (int i = 0; i < numberOfCells; i++)
            {
                Color color = Color.FromScRgb(1.0f, (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                SolidColorBrush fill = new SolidColorBrush(color);
                fill.Freeze();

                System.Windows.Shapes.Rectangle child = new System.Windows.Shapes.Rectangle();
                child.Fill = fill;

                grid.Children.Add(child);
            }

            return grid;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return (_uniformGrid != null) ? 1 : 0;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if ((_uniformGrid == null) || (index != 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return _uniformGrid;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (_uniformGrid != null)
            {
                _uniformGrid.Measure(constraint);
                return _uniformGrid.DesiredSize;
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (_uniformGrid != null)
            {
                _uniformGrid.Arrange(new Rect(arrangeSize));
            }
            return arrangeSize;
        }

        private UniformGrid _uniformGrid = null;
    }
}
