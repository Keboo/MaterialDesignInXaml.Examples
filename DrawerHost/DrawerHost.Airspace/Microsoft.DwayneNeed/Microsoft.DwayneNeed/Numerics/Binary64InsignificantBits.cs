using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     Contains an integer between 0 and 52.
    /// </summary>
    public struct Binary64InsignificantBits
    {
        public Binary64InsignificantBits(uint insignificantBits)
            : this()
        {
            if (insignificantBits > 52)
            {
                throw new ArgumentOutOfRangeException("significandBits");
            }

            Value = insignificantBits;
        }

        public static implicit operator uint(Binary64InsignificantBits value)
        {
            return value.Value;
        }

        public static implicit operator Binary64InsignificantBits(uint value)
        {
            return new Binary64InsignificantBits(value);
        }

        public static implicit operator int(Binary64InsignificantBits value)
        {
            return (int) value.Value;
        }

        public static implicit operator Binary64InsignificantBits(int value)
        {
            return new Binary64InsignificantBits((uint) value);
        }

        public uint Value { get; private set; }
    }
}
