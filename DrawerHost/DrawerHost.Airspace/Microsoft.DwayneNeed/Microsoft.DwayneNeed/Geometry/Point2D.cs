using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 2-dimensional point. 
    /// </summary>
    public interface IPoint2D<T>
    {
        T X { get; }
        T Y { get; }
    }

    /// <summary>
    ///     A simple implementation of a 2-dimensional point.
    /// </summary>
    public struct Point2D<T> : IPoint2D<T>
    {
        public Point2D(T x, T y) : this()
        {
            X = x;
            Y = y;
        }

        public T X { get; private set; }
        public T Y { get; private set; }
    }
}
