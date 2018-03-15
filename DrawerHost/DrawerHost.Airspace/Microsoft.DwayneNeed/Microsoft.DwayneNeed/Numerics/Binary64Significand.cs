using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     A numeric type for representing a fraction.
    /// </summary>
    public struct Binary64Significand
    {
        public Binary64Significand(ulong value, bool isSubNormal) : this()
        {
            // Only 52 bits can be specified.
            ulong mask = ((ulong)1 << 52) - 1;
            if ((value & ~mask) != 0)
            {
                throw new ArgumentOutOfRangeException("significand");
            }

            Value = value;
            IsSubnormal = isSubNormal;
        }

        public ulong Value { get; private set;}
        public bool IsSubnormal { get; private set; }

        public double Fraction
        {
            get
            {
                ulong denominator = (ulong)1 << 52;
                ulong numerator = Value;
                if (!IsSubnormal)
                {
                    numerator += denominator;
                }

                return (double)numerator / (double)denominator;
            }
        }
    }
}
