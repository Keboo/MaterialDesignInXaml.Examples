using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Markup;
using Microsoft.DwayneNeed.Media;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Shapes
{
    public sealed class Cone : ParametricShape3D
    {
        static Cone()
        {
            // The height of the cone is specified by MaxV, so make the default
            // MaxV property be 1.
            ParametricShape3D.MaxVProperty.OverrideMetadata(typeof(Cone), new PropertyMetadata(1.0));
        }

        public static DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                "Radius",
                typeof(double),
                typeof(Cone), new PropertyMetadata(
                    0.5, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        protected override System.Windows.Media.Media3D.Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double radius = Radius;
            double height = MaxV-MinV;

            double x = (v.Value * radius * u.Cos) / height;
            double y = (v.Value * radius * u.Sin) / height; 
            double z = v.Value;

            return new Point3D(x, y, z);
        }
    }
}
