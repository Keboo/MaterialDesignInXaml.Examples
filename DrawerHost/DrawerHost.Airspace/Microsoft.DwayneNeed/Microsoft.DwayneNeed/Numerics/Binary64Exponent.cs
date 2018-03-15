using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     The Binary64Exponent struct represents the exponent of a
    ///     double-precision IEEE 754 floating point number.
    /// </summary>
    /// <remarks>
    ///     The IEEE 754 standard calls for biasing the exponent by 1023.
    ///     Biasing provides a simple scheme for encoding both positive and
    ///     negative numbers in an unsigned field.
    /// </remarks>
    public struct Binary64Exponent
    {
        public Binary64Exponent(uint exponent)
        {
            if (exponent > 2047)
            {
                throw new ArgumentOutOfRangeException("exponent");
            }

            _exponent = exponent;
        }

        public uint UnbiasedValue { get { return _exponent; } }
        public int BiasedValue { get { return (int)_exponent - (int)Bias; } }
        public Sign Sign { get { return new Sign(_exponent < Bias); } }

        public const uint Bias = 1023;

        uint _exponent;
    }
}
