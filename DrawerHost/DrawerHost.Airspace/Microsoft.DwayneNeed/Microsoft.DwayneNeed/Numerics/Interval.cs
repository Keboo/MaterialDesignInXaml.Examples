using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     An interface representing a connected portion of the real line
    ///     where each of the endpoints can be open or closed.
    /// </summary>
    public interface IInterval<T>
    {
        T Min { get; }
        bool IsMinClosed { get; }

        T Max { get; }
        bool IsMaxClosed { get; }
    }

    /// <summary>
    ///     A simple interval implementation that can have endpoints that are
    ///     either open or closed.
    /// </summary>
    public struct Interval<T> : IInterval<T>
    {
        public Interval(T min, bool isMinClosed, T max, bool isMaxClosed) : this()
        {
            Min = min;
            IsMinClosed = isMinClosed;
            Max = max;
            IsMaxClosed = isMaxClosed;
        }

        public T Min {get; private set;}
        public bool IsMinClosed {get; private set;}

        public T Max {get; private set;}
        public bool IsMaxClosed {get; private set;}

        public override string ToString()
        {
            return String.Format("{0}{1},{2}{3}",
                IsMinClosed ? "(" : "[",
                Min,
                Max,
                IsMaxClosed ? ")" : "]");
        }
    }
}
