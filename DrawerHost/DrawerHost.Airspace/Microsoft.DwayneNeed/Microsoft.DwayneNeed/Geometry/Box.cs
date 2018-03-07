using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A Box is the generalization of a rectangle for higher dimensions,
    ///     formally defined as the Cartesian product of intervals.
    /// </summary>
    public interface IBox<T>
    {
        int Dimensions { get; }
        Interval<T> GetInterval(int dimension);
    }

    /// <summary>
    ///     A simple implementation of an n-dimensional box.
    /// </summary>
    public struct Box<T> : IBox<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point">
        ///     An n-dimensional point for one "corner".
        /// </param>
        /// <param name="vector">
        ///     An n-dimensional vector to the opposite "corner".
        /// </param>
        public Box(IPoint<T> point, IVector<T> vector)
        {
            int dimensions = point.Dimensions;
            if (dimensions != vector.Dimensions)
            {
                throw new ArgumentException("The point and the vector must agree on dimensionality.");
            }

            _intervals = new Interval<T>[dimensions];
            for(int dimension = 0; dimension < dimensions; dimension++)
            {
                dynamic start = point.GetCoordinate(dimension);
                dynamic end = start + vector.GetDelta(dimension);
                if (start <= end)
                {
                    _intervals[dimension] = new Interval<T>(start, true, end, true);
                }
                else
                {
                    _intervals[dimension] = new Interval<T>(end, true, start, true);
                }
            }
        }

        public Box(params Interval<T>[] intervals) : this((IEnumerable<Interval<T>>) intervals)
        {
        }

        public Box(IEnumerable<Interval<T>> intervals)
        {
            _intervals = intervals.ToArray();
        }

        public int Dimensions
        {
            get
            {
                return _intervals == null ? 0 : _intervals.Length;
            }
        }

        public Interval<T> GetInterval(int dimension)
        {
            if (dimension < 0 || _intervals == null || dimension >= _intervals.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _intervals[dimension];
        }

        private Interval<T>[] _intervals;
    }
}
