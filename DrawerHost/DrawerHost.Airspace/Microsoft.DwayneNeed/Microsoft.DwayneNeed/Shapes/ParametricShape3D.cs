using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Markup;
using Microsoft.DwayneNeed.Media;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.DwayneNeed.Extensions;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Shapes
{
    /// <summary>
    ///     ParametricShape3D assumes responsibility for generating the
    ///     mesh for all parametric surfaces.  This is done by sampling the
    ///     UV plane at regular intervals, projecting these UV points into
    ///     3D space by calling a virtual method that derived types can
    ///     implement, and then finally constructing triangles between the
    ///     points.
    ///     
    ///     Texture coordinates are simply the (u,v) coordinates scaled
    ///     between [0,1].
    ///     
    ///     Normals are calculated in one of two ways:
    ///     1) Faceted
    ///        The number of triangles is doubled, and each vertex of a
    ///        given triangle has the same normal, which is the "face"
    ///        normal.
    ///     2) Smooth
    ///        There is one triangle for every 3 points, and each vertex
    ///        has a different vertex, calculates as the average of the
    ///        adjacent "face" normals.
    /// </summary>
    public abstract class ParametricShape3D : Shape3D
    {
        static ParametricShape3D()
        {
            FrontMaterialProperty.OverrideMetadata(typeof(ParametricShape3D),
                new PropertyMetadata(new Shape3DMaterial(new Func<Shape3D, UIElement>(GetDefaultFrontMaterial))));

            BackMaterialProperty.OverrideMetadata(typeof(ParametricShape3D),
                new PropertyMetadata(new Shape3DMaterial(new Func<Shape3D, UIElement>(GetDefaultBackMaterial))));
        }

        public static DependencyProperty MinUProperty =
            DependencyProperty.Register(
                "MinU",
                typeof(double),
                typeof(ParametricShape3D),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double MinU
        {
            get { return (double)GetValue(MinUProperty); }
            set { SetValue(MinUProperty, value); }
        }

        public static DependencyProperty MaxUProperty =
            DependencyProperty.Register(
                "MaxU",
                typeof(double),
                typeof(ParametricShape3D),
                new PropertyMetadata(Math.PI*2, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double MaxU
        {
            get { return (double)GetValue(MaxUProperty); }
            set { SetValue(MaxUProperty, value); }
        }

        public static DependencyProperty DivUProperty =
            DependencyProperty.Register(
                "DivU",
                typeof(int),
                typeof(ParametricShape3D),
                new PropertyMetadata(32, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public int DivU
        {
            get { return (int)GetValue(DivUProperty); }
            set { SetValue(DivUProperty, value); }
        }

        public static DependencyProperty MinVProperty =
            DependencyProperty.Register(
                "MinV",
                typeof(double),
                typeof(ParametricShape3D),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double MinV
        {
            get { return (double)GetValue(MinVProperty); }
            set { SetValue(MinVProperty, value); }
        }

        public static DependencyProperty MaxVProperty =
            DependencyProperty.Register(
                "MaxV",
                typeof(double),
                typeof(ParametricShape3D),
                new PropertyMetadata(Math.PI, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double MaxV
        {
            get { return (double)GetValue(MaxVProperty); }
            set { SetValue(MaxVProperty, value); }
        }

        public static DependencyProperty DivVProperty =
            DependencyProperty.Register(
                "DivV",
                typeof(int),
                typeof(ParametricShape3D),
                new PropertyMetadata(32, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public int DivV
        {
            get { return (int)GetValue(DivVProperty); }
            set { SetValue(DivVProperty, value); }
        }

        protected abstract Point3D Project(MemoizeMath u, MemoizeMath v);

        protected override sealed MeshGeometry3D GetGeometry()
        {
            double minU = MinU;
            double maxU = MaxU;
            int divU = DivU;
            double minV = MinV;
            double maxV = MaxV;
            int divV = DivV;

            double spanU = (maxU - minU);
            double spanV = (maxV - minV);
            int stride = divU+1;

            MeshGeometry3D mesh = new MeshGeometry3D();
            
            // Create memoized wrappers for each of the u and v divisions.
            // This is a massive performance improvement in time complexity.
            List<MemoizeMath> uDivisions = new List<MemoizeMath>();
            for (int iU = 0; iU <= divU; iU++)
            {
                double u = ((spanU * iU) / divU) + minU;
                uDivisions.Add(new MemoizeMath(u));
            }

            List<MemoizeMath> vDivisions = new List<MemoizeMath>();
            for (int iV = 0; iV <= divV; iV++)
            {
                double v = ((spanV * iV) / divV) + minV;
                vDivisions.Add(new MemoizeMath(v));
            }

            // Iterate through the (u,v) space and sample the parametric surface.
            foreach(MemoizeMath v in vDivisions)
            {
                foreach(MemoizeMath u in uDivisions)
                {
                    // Project the (u,v) points into 3D space.
                    Point3D position = Project(u,v);
                    mesh.Positions.Add(position);
                }
            }

            // Assign texture coordinates based on (u,v) position such
            // that they are in the range of [0,1].
            for (int iV = 0; iV <= divV; iV++)
            {
                for (int iU = 0; iU <= divU; iU++)
                {
                    double tu = iU/(double)divU;
                    double tv = iV/(double)divV;
                    mesh.TextureCoordinates.Add(new Point(tu, tv));
                }
            }

            // Iterate through the positions in the mesh updating related properties.
            double maxQuadWidth = 0;
            double maxQuadHeight = 0;
            int iPosition = 0;
            bool isNWSE = false;

            for (int iV = 0; iV < divV; iV++)
            {
                bool oldIsNWSE = isNWSE;

                for (int iU = 0; iU < divU; iU++)
                {
                    // Get the width & height of this quad, keep track of the
                    // max width and height of each quad.
                    Vector3D uStep = mesh.Positions[iPosition + 1] - mesh.Positions[iPosition];
                    Vector3D vStep = mesh.Positions[iPosition + stride] - mesh.Positions[iPosition];
                    maxQuadWidth = Math.Max(uStep.Length, maxQuadWidth);
                    maxQuadHeight = Math.Max(vStep.Length, maxQuadHeight);

                    // Process each quad in the (u,v) grid into two triangles.
                    var quad = new Tuple<int, int, int, int>(iPosition, iPosition + stride, iPosition + stride + 1, iPosition + 1);
                    Tuple<int,int,int> tri1, tri2;
                    GetTrianglesFromQuad(quad, isNWSE, out tri1, out tri2);

                    mesh.TriangleIndices.Add(tri1);
                    mesh.TriangleIndices.Add(tri2);

                    // Calculate normals...TODO

                    iPosition++;
                    isNWSE = !isNWSE;
                }

                iPosition++;
                isNWSE = !oldIsNWSE;
            }

            // Calculate the effective size of the surface based on the maximum
            // quad size.  This represents a surface that will have full
            // fidelity at the most distorted portions of the mesh.  The
            // portions of the mesh with the least distortion will have more
            // texture to sample from than needed.  Unfortunately, BitmapCache
            // does not support mip-mapping so this can lead to some speckles.
            //
            // TODO: register a default value callback...
            SurfaceWidth = maxQuadWidth * divU;
            SurfaceHeight = maxQuadHeight * divU;

            //mesh.Freeze();

            return mesh;
        }

        private static void GetTrianglesFromQuad(Tuple<int, int, int, int> quad, bool isNWSE, out Tuple<int, int, int> tri1, out Tuple<int, int, int> tri2)
        {
            if (isNWSE)
            {
                tri1 = new Tuple<int, int, int>(quad.Item1, quad.Item2, quad.Item3);
                tri2 = new Tuple<int, int, int>(quad.Item3, quad.Item4, quad.Item1);
            }
            else
            {
                tri1 = new Tuple<int, int, int>(quad.Item4, quad.Item1, quad.Item2);
                tri2 = new Tuple<int, int, int>(quad.Item2, quad.Item3, quad.Item4);
            }
        }
        private static UIElement GetDefaultFrontMaterial(Shape3D shape)
        {
            ParametricShape3D _this = (ParametricShape3D)shape;
            return _this.GetDefaultMaterial(true);
        }

        private static UIElement GetDefaultBackMaterial(Shape3D shape)
        {
            ParametricShape3D _this = (ParametricShape3D)shape;
            return _this.GetDefaultMaterial(false);
        }

        private UIElement GetDefaultMaterial(bool front)
        {
            Grid grid = new Grid();

            int divV = DivV;
            for (int iV = 0; iV < divV; iV++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }

            int divU = DivU;
            for (int iU = 0; iU < divU; iU++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(col);
            }

            bool useColorAtRowStart = true;
            bool useColor;
            for (int iV = 0; iV < divV; iV++)
            {
                useColor = useColorAtRowStart;
                useColorAtRowStart = !useColorAtRowStart;

                for (int iU = 0; iU < divU; iU++)
                {
                    UIElement thing;

                    Border b = new Border();
                    b.Background = useColor ? (front ? Brushes.Red : Brushes.AliceBlue) : Brushes.Black;
                    //b.BorderThickness = new Thickness(1.0);
                    //b.BorderBrush = Brushes.Black;

                    thing = b;

                    Grid.SetRow(thing, iV);
                    Grid.SetColumn(thing, iU);
                    grid.Children.Add(thing);

                    useColor = !useColor;
                }
            }

            return grid;
        }
    }
}
