using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     An interface representing a connected portion of the real line
    ///     where each of the endpoints is open.
    /// </summary>
    public interface IOpenInterval<T>
    {
        T Min { get; }
        T Max { get; }
    }

    /// <summary>
    ///     A simple implementation of an open interval.
    /// </summary>
    public struct OpenInterval<T> : IOpenInterval<T>
    {
        public T Min { get; private set; }
        public T Max { get; private set; }

        // Use the standard notation for intervals.
        public override string ToString()
        {
            return String.Format("[{0},{1}]",
                Min,
                Max);
        }
    }
}
