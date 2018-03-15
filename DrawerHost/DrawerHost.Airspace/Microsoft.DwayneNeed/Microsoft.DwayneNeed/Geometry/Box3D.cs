using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Numerics;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 3-dimensional box. 
    /// </summary>
    public interface IBox3D<T>
    {
        Interval<T> Width { get; }
        Interval<T> Height { get; }
        Interval<T> Depth { get; }
    }

    /// <summary>
    ///     A simple implementation of a 3-dimensional box.
    /// </summary>
    public struct Box3D<T> : IBox3D<T>
    {
        public Box3D(IPoint3D<T> point, IVector3D<T> vector)
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

            dynamic startDepth = point.Z;
            dynamic endDepth = startDepth + vector.DeltaZ;
            if (startDepth <= endDepth)
            {
                Depth = new Interval<T>(startDepth, true, endDepth, true);
            }
            else
            {
                Depth = new Interval<T>(endDepth, true, startDepth, true);
            }
        }

        public Box3D(Interval<T> width, Interval<T> height, Interval<T> depth)
            : this()
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public Interval<T> Width { get; private set; }
        public Interval<T> Height { get; private set; }
        public Interval<T> Depth { get; private set; }
    }
}
