using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    public static class BoxExtensions
    {
        public static IBox<T> AsBox<T>(this IBox1D<T> box1D)
        {
            return new Box<T>(box1D.Width);
        }

        public static IBox<T> AsBox<T>(this IBox2D<T> box2D)
        {
            return new Box<T>(box2D.Width, box2D.Height);
        }

        public static IBox<T> AsBox<T>(this IBox3D<T> box3D)
        {
            return new Box<T>(box3D.Width, box3D.Height, box3D.Depth);
        }
    }
}
