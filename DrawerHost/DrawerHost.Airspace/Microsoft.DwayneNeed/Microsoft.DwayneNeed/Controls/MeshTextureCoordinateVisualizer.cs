using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Microsoft.DwayneNeed.Controls
{
    /// <summary>
    ///     Renders lines connecting the texture coordinates for each triangle
    ///     in a mesh.  This can be rendered on top of the texture to show the
    ///     triangular mesh.
    /// </summary>
    class MeshTextureCoordinateVisualizer : FrameworkElement
    {
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Mesh", 
                                        typeof(MeshGeometry3D),
                                        typeof(MeshTextureCoordinateVisualizer),
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public MeshGeometry3D Mesh
        {
            get { return (MeshGeometry3D)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double width = RenderSize.Width;
            double height = RenderSize.Height;
            MeshGeometry3D mesh = Mesh;

            if (mesh != null)
            {
                Pen pen = new Pen(Brushes.Black, 1.0);

                int numTriangles = mesh.TriangleIndices.Count/3;

                for(int i = 0; i < numTriangles; i++)
                {
                    DrawTriangle(drawingContext,
                                 pen,
                                 mesh.TextureCoordinates[mesh.TriangleIndices[i * 3]],
                                 mesh.TextureCoordinates[mesh.TriangleIndices[i * 3 + 1]],
                                 mesh.TextureCoordinates[mesh.TriangleIndices[i * 3 + 2]],
                                 width,
                                 height);
                }
            }

            base.OnRender(drawingContext);
        }

        private static void DrawTriangle(DrawingContext drawingContext, Pen pen, Point a, Point b, Point c, double width, double height)
        {
            Point ta = new Point(a.X * width, a.Y * height);
            Point tb = new Point(b.X * width, b.Y * height);
            Point tc = new Point(c.X * width, c.Y * height);

            drawingContext.DrawLine(pen, ta, tb);
            drawingContext.DrawLine(pen, tb, tc);
            drawingContext.DrawLine(pen, tc, ta);
        }
    }
}
