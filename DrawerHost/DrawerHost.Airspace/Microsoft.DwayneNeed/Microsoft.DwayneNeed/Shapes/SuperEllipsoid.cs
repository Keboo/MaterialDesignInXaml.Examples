using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Microsoft.DwayneNeed.Media;
using System.Windows;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Shapes
{
    public class SuperEllipsoid : ParametricShape3D
    {
        public static DependencyProperty N1Property =
            DependencyProperty.Register(
                "N1",
                typeof(double),
                typeof(SuperEllipsoid), new PropertyMetadata(
                    4.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double N1
        {
            get { return (double)GetValue(N1Property); }
            set { SetValue(N1Property, value); }
        }

        public static DependencyProperty N2Property =
            DependencyProperty.Register(
                "N2",
                typeof(double),
                typeof(SuperEllipsoid), new PropertyMetadata(
                    4.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double N2
        {
            get { return (double)GetValue(N2Property); }
            set { SetValue(N2Property, value); }
        }

        protected override Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double xRadius = 1.0;
            double yRadius = 1.0;
            double zRadius = 1.0;
            double n1 = N1;
            double n2 = N2;

            double x = xRadius * SafePow(v.Sin, n1) * SafePow(u.Cos, n2);
            double y = yRadius * SafePow(v.Sin, n1) * SafePow(u.Sin, n2);
            double z = zRadius * SafePow(v.Cos, n1);
            return new Point3D(x, y, z);
        }

        private static double SafePow(double value, double power)
        {
            if (value > 0)
            {
                return Math.Pow(value, power);
            }
            else if (value < 0)
            {
                return -Math.Pow(-value, power);
            }
            else
            {
                return 0;
            }
        }
    }
}
