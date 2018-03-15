using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 3-dimensional vector. 
    /// </summary>
    public interface IVector3D<T>
    {
        T DeltaX { get; }
        T DeltaY { get; }
        T DeltaZ { get; }
    }

    /// <summary>
    ///     A simple implementation of a 3-dimensional vector.
    /// </summary>
    public struct Vector3D<T> : IVector3D<T>
    {
        public Vector3D(T deltaX, T deltaY, T deltaZ)
            : this()
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
        }

        public T DeltaX { get; private set; }
        public T DeltaY { get; private set; }
        public T DeltaZ { get; private set; }
    }
}
