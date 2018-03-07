using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Shapes
{
    public class SuperToroid : ParametricShape3D
    {
        static SuperToroid()
        {
            // So texture coordinates work out better, configure the default
            // MinV property to be PI.
            ParametricShape3D.MinVProperty.OverrideMetadata(typeof(SuperToroid), new PropertyMetadata(Math.PI));

            // So texture coordinates work out better, configure the default
            // MaxV property to be 3*PI.
            ParametricShape3D.MaxVProperty.OverrideMetadata(typeof(SuperToroid), new PropertyMetadata(Math.PI * 3.0));
        }

        public static DependencyProperty N1Property =
            DependencyProperty.Register(
                "N1",
                typeof(double),
                typeof(SuperToroid), new PropertyMetadata(
                    2.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double N1
        {
            get { return (double)GetValue(N1Property); }
            set { SetValue(N1Property, value); }
        }

        public static DependencyProperty N2Property =
            DependencyProperty.Register(
                "N2",
                typeof(double),
                typeof(SuperToroid), new PropertyMetadata(
                    2.0, new PropertyChangedCallback(OnPropertyChangedAffectsModel)));

        public double N2
        {
            get { return (double)GetValue(N2Property); }
            set { SetValue(N2Property, value); }
        }

        protected override System.Windows.Media.Media3D.Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double centerRadius = 2.0;
            double crossSectionRadius = 1.0;
            double n1 = N1;
            double n2 = N2;

            double x = (centerRadius + crossSectionRadius * SafePow(v.Cos, N2)) * SafePow(Math.Cos(-u.Value), n1);
            double y = (centerRadius + crossSectionRadius * SafePow(v.Cos, N2)) * SafePow(Math.Sin(-u.Value), n1);
            double z = crossSectionRadius * SafePow(v.Sin, n2);
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
