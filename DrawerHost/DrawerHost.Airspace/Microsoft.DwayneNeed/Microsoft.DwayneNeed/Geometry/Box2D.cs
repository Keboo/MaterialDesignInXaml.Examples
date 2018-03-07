using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 2-dimensional box. 
    /// </summary>
    public interface IBox2D<T>
    {
        Interval<T> Width { get; }
        Interval<T> Height { get; }
    }

    /// <summary>
    ///     A simple implementation of a 2-dimensional box.
    /// </summary>
    public struct Box2D<T> : IBox2D<T>
    {
        public Box2D(IPoint2D<T> point, IVector2D<T> vector)
            : this()
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

            dynamic startHeight = point.Y;
            dynamic endHeight = startHeight + vector.DeltaY;
            if (startHeight <= endHeight)
            {
                Height = new Interval<T>(startHeight, true, endHeight, true);
            }
            else
            {
                Height = new Interval<T>(endHeight, true, startHeight, true);
            }
        }

        public Box2D(Interval<T> width, Interval<T> height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public Interval<T> Width { get; private set; }
        public Interval<T> Height { get; private set; }
    }
}
