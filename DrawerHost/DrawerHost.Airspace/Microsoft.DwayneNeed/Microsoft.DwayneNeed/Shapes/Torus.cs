using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Shapes
{
    public class Torus : ParametricShape3D
    {
        static Torus()
        {
            // So texture coordinates work out better, configure the default
            // MinV property to be PI.
            ParametricShape3D.MinVProperty.OverrideMetadata(typeof(Torus), new PropertyMetadata(Math.PI));

            // So texture coordinates work out better, configure the default
            // MaxV property to be 3*PI.
            ParametricShape3D.MaxVProperty.OverrideMetadata(typeof(Torus), new PropertyMetadata(Math.PI * 3.0));
        }

        protected override System.Windows.Media.Media3D.Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double centerRadius = 2.0;
            double crossSectionRadius = 1.0;

            double x = (centerRadius + crossSectionRadius * v.Cos) * Math.Cos(-u.Value);
            double y = (centerRadius + crossSectionRadius * v.Cos) * Math.Sin(-u.Value);
            double z = crossSectionRadius * v.Sin;
            return new Point3D(x, y, z);
        }
    }
}
