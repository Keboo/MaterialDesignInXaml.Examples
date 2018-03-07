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
    public sealed class RomanSurface : ParametricShape3D
    {
        public static DependencyProperty AProperty =
            DependencyProperty.Register(
                "A",
                typeof(double),
                typeof(RomanSurface), new PropertyMetadata(
                    1.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double A
        {
            get { return (double)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }

        protected override Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double a = A;

            double x = (a * a * Math.Sin(2.0 * u.Value) * Math.Pow(v.Cos, 2)) / 2.0;
            double y = (a * a * u.Sin * Math.Sin(v.Value * 2.0)) / 2.0;
            double z = (a * a * u.Cos * Math.Sin(v.Value * 2.0)) / 2.0;
            return new Point3D(x, y, z);
        }
    }
}
