using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 1-dimensional point. 
    /// </summary>
    public interface IPoint1D<T>
    {
        T X { get; }
    }

    /// <summary>
    ///     A simple implementation of a 1-dimensional point.
    /// </summary>
    public struct Point1D<T> : IPoint1D<T>
    {
        public Point1D(T x) : this()
        {
            X = x;
        }

        public T X { get; private set; }
    }
}
