using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 1-dimensional box. 
    /// </summary>
    public interface IBox1D<T>
    {
        Interval<T> Width { get; }
    }

    /// <summary>
    ///     A simple implementation of a 1-dimensional box.
    /// </summary>
    public struct Box1D<T> : IBox1D<T>
    {
        public Box1D(IPoint1D<T> point, IVector1D<T> vector) : this()
        {
            dynamic startWidth = point.X;
            dynamic endWidth = startWidth + vector.DeltaX;
            if (startWidth <= endWidth)
            {
                Width = new Interval<T>(startWidth, true, endWidth, true);
            }
            else
            {
                Width = new Interval<T>(endWidth, true, startWidth, true);
            }
        }

        public Box1D(Interval<T> width) : this()
        {
            Width = width;
        }

        public Interval<T> Width { get; private set; }
    }
}
