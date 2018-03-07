using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    public static class VectorExtensions
    {
        public static IVector<T> AsVector<T>(this IVector1D<T> vector1D)
        {
            return new Vector<T>(vector1D.DeltaX);
        }

        public static IVector<T> AsVector<T>(this IVector2D<T> vector2D)
        {
            return new Vector<T>(vector2D.DeltaX, vector2D.DeltaY);
        }

        public static IVector<T> AsVector<T>(this IVector3D<T> vector3D)
        {
            return new Vector<T>(vector3D.DeltaX, vector3D.DeltaY, vector3D.DeltaZ);
        }
    }
}
