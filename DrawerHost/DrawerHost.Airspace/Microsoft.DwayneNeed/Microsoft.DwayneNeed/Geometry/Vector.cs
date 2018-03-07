using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for an n-dimensional vector. 
    /// </summary>
    public interface IVector<T>
    {
        int Dimensions { get; }
        T GetDelta(int dimension);
    }

    /// <summary>
    ///     A simple implementation of an n-dimensional vector.
    /// </summary>
    public struct Vector<T> : IVector<T>
    {
        public Vector(params T[] deltas)
            : this((IEnumerable<T>)deltas)
        {
        }

        public Vector(IEnumerable<T> deltas)
        {
            _deltas = deltas.ToArray();
        }

        public int Dimensions
        {
            get
            {
                return _deltas == null ? 0 : _deltas.Length;
            }
        }

        public T GetDelta(int dimension)
        {
            if (dimension < 0 || _deltas == null || dimension >= _deltas.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _deltas[dimension];
        }

        private T[] _deltas;
    }
}
