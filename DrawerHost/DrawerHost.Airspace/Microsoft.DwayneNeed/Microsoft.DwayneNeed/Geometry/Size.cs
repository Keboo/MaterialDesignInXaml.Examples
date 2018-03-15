using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for an n-dimensional size. 
    /// </summary>
    /// <remarks>
    ///     Size is similar to Vector, except that the extents cannot be
    ///     negative.
    /// </remarks>
    public interface ISize<T>
    {
        int Dimensions { get; }
        T GetExtent(int dimension);
    }

    /// <summary>
    ///     A simple implementation of an n-dimensional size.
    /// </summary>
    public struct Size<T> : ISize<T>
    {
        public Size(params T[] extents) : this((IEnumerable<T>) extents)
        {
        }

        public Size(IEnumerable<T> extents)
        {
            T nZero = default(T);

            foreach(T extent in extents)
            {
                dynamic nExtent = extent;
                if(nExtent < nZero)
                {
                    throw new InvalidOperationException("Size extents may not be negative.");
                }
            }

            _extents = extents.ToArray();
        }

        public int Dimensions
        {
            get
            {
                return _extents == null ? 0 : _extents.Length;
            }
        }

        public T GetExtent(int dimension)
        {
            if (dimension < 0 || _extents == null || dimension >= _extents.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _extents[dimension];
        }

        private T[] _extents;
    }
}
