using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    public static class IntervalExtensions
    {
        // Support converting any ICloseInterval to an IInterval
        public static IInterval<T> AsInterval<T>(this IClosedInterval<T> closedInterval)
        {
            return new Interval<T>(closedInterval.Min, true, closedInterval.Max, true);
        }

        // Support converting any IOpenInterval to an IInterval
        public static IInterval<T> AsInterval<T>(this IOpenInterval<T> openInterval)
        {
            return new Interval<T>(openInterval.Min, false, openInterval.Max, false);
        }
    }
}
