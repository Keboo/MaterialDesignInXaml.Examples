using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 3-dimensional size. 
    /// </summary>
    public interface ISize3D<T>
    {
        T Width { get; }
        T Height { get; }
        T Depth { get; }
    }

    /// <summary>
    ///     A simple implementation of a 3-dimensional size.
    /// </summary>
    public struct Size3D<T> : ISize3D<T>
    {
        public Size3D(T width, T height, T depth) : this()
        {
            T nZero = default(T);
            dynamic nWidth = width;
            dynamic nHeight = height;
            dynamic nDepth = depth;
            if (nWidth < nZero || nHeight < nZero || nDepth < nZero)
            {
                throw new InvalidOperationException("Size extents may not be negative.");
            }

            Width = width;
            Height = height;
            Depth = depth;
        }

        public T Width { get; private set; }
        public T Height { get; private set; }
        public T Depth { get; private set; }
    }
}
