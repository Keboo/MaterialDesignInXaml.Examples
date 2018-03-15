using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 1-dimensional vector. 
    /// </summary>
    public interface IVector1D<T>
    {
        T DeltaX { get; }
    }

    /// <summary>
    ///     A simple implementation of a 1-dimensional vector.
    /// </summary>
    public struct Vector1D<T> : IVector1D<T>
    {
        public Vector1D(T deltaX) : this()
        {
            DeltaX = deltaX;
        }

        public T DeltaX { get; private set; }
    }
}
