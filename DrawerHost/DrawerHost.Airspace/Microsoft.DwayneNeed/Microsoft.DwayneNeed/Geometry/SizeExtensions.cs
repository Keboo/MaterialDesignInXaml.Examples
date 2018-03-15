using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    public static class SizeExtensions
    {
        public static ISize<T> AsSize<T>(this ISize1D<T> size1D)
        {
            return new Size<T>(size1D.Length);
        }

        public static ISize<T> AsSize<T>(this ISize2D<T> size2D)
        {
            return new Size<T>(size2D.Width, size2D.Height);
        }

        public static ISize<T> AsSize<T>(this ISize3D<T> size3D)
        {
            return new Size<T>(size3D.Width, size3D.Height, size3D.Depth);
        }
    }
}
