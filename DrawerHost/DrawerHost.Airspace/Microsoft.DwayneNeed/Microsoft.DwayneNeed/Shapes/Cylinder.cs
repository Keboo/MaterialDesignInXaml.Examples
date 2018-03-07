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
    public sealed class Cylinder : ParametricShape3D
    {
        static Cylinder()
        {
            // The height of the cylinder is specified by MaxV, so make the
            // default MaxV property be 1.
            ParametricShape3D.MaxVProperty.OverrideMetadata(typeof(Cylinder), new PropertyMetadata(1.0));
        }

        protected override System.Windows.Media.Media3D.Point3D Project(MemoizeMath u, MemoizeMath v)
        {
            double radius = 1;

            double x = (radius * u.Sin);
            double y = (radius * u.Cos);
            double z = v.Value;
            return new Point3D(x, y, z);
        }
    }
}
