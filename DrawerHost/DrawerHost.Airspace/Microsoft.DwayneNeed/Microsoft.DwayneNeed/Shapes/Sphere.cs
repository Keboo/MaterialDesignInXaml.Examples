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
    public sealed class Sphere : ParametricShape3D
    {
        protected override Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double radius = 1.0;

            double x = radius * u.Cos * v.Sin;
            double y = radius * u.Sin * v.Sin;
            double z = radius * v.Cos;
            return new Point3D(x, y, z);
        }
    }
}
