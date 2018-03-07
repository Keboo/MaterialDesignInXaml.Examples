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
    public class Ellipsoid : ParametricShape3D
    {
        protected override Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double xRadius = 1.0;
            double yRadius = 1.0;
            double zRadius = 1.0;

            double x = xRadius * u.Cos * v.Sin;
            double y = yRadius * u.Sin * v.Sin;
            double z = zRadius * v.Cos;
            return new Point3D(x, y, z);
        }
    }
}
