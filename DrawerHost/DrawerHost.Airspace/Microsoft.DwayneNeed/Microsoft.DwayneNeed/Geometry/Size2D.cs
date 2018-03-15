using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 2-dimensional size. 
    /// </summary>
    public interface ISize2D<T>
    {
        T Width { get; }
        T Height { get; }
    }

    /// <summary>
    ///     A simple implementation of a 2-dimensional size.
    /// </summary>
    public struct Size2D<T> : ISize2D<T>
    {
        public Size2D(T width, T height) : this()
        {
            T nZero = default(T);
            dynamic nWidth = width;
            dynamic nHeight = height;
            if (nWidth < nZero || nHeight < nZero)
            {
                throw new InvalidOperationException("Size extents may not be negative.");
            }

            Width = width;
            Height = height;
        }

        public T Width { get; private set; }
        public T Height { get; private set; }
    }
}
