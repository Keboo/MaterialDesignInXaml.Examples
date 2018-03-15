using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 1-dimensional size. 
    /// </summary>
    public interface ISize1D<T>
    {
        T Length { get; }
    }

    /// <summary>
    ///     A simple implementation of a 1-dimensional size.
    /// </summary>
    public struct Size1D<T> : ISize1D<T>
    {
        public Size1D(T length) : this()
        {
            T nZero = default(T);
            dynamic nLength = length;
            if (nLength < nZero)
            {
                throw new InvalidOperationException("Size extents may not be negative.");
            }

            Length = length;
        }

        public T Length { get; private set; }
    }
}
