using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCV.RectangleDetector
{
    internal static class MathMixins
    {
        //Based on http://docs.scipy.org/doc/numpy/reference/generated/numpy.median.html
        public static double Median(this IEnumerable<byte> values)
        {
            return Median(values, (a, b) => (a + b) / 2.0);
        }

        private static double Median<T>(IEnumerable<T> values, Func<T, T, double> calcMedian)
        {
            var a = values.OrderBy(x => x).ToArray();
            double middle = a.Length / 2.0;
            //Average the new "middle" numbers
            //When a.Length is odd both the Floor and Ceiling will be the same value
            return calcMedian(a[(int) Math.Floor(middle)], a[(int) Math.Ceiling(middle)]);
        }
    }
}