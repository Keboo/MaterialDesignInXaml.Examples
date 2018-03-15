using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    public static class PointExtensions
    {
        public static IPoint<T> AsPoint<T>(this IPoint1D<T> point1D)
        {
            return new Point<T>(point1D.X);
        }

        public static IPoint<T> AsPoint<T>(this IPoint2D<T> point2D)
        {
            return new Point<T>(point2D.X, point2D.Y);
        }

        public static IPoint<T> AsPoint<T>(this IPoint3D<T> point3D)
        {
            return new Point<T>(point3D.X, point3D.Y, point3D.Z);
        }
    }
}
