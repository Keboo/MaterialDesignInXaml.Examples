using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 2-dimensional vector. 
    /// </summary>
    public interface IVector2D<T>
    {
        T DeltaX { get; }
        T DeltaY { get; }
    }

    /// <summary>
    ///     A simple implementation of a 2-dimensional point.
    /// </summary>
    public struct Vector2D<T> : IVector2D<T>
    {
        public Vector2D(T deltaX, T deltaY) : this()
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public T DeltaX { get; private set; }
        public T DeltaY { get; private set; }
    }
}
