using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     Struct exposing the binary64 floating point format as specified by
    ///     the IEEE Standard for Floating-Point Arithmetic (IEEE 754).
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Binary64
    {
        public Binary64(double value)
            : this()
        {
            _value = value;
        }

        public Binary64(ulong bits)
            : this()
        {
            _bits = bits;
        }

        public static implicit operator double(Binary64 value)
        {
            return value._value;
        }

        public static implicit operator Binary64(double value)
        {
            return new Binary64(value);
        }

        public static implicit operator ulong(Binary64 value)
        {
            return value._bits;
        }

        public static implicit operator Binary64(ulong value)
        {
            return new Binary64(value);
        }

        public ulong Bits { get { return _bits; } }
        
        public double Value { get { return _value; } }

        public Sign Sign
        {
            get
            {
                return new Sign((_bits & BITS_SIGN) != 0);
            }
        }

        public Binary64Exponent Exponent
        {
            get
            {
                return new Binary64Exponent((uint)((_bits & BITS_EXPONENT) >> 52));
            }
        }
        
        public Binary64Significand Significand
        {
            get
            {
                return new Binary64Significand(_bits & BITS_SIGNIFICAND, (_bits & BITS_EXPONENT) == 0);
            }
        }

        public bool IsInfinite
        {
            get
            {
                return (_bits & BITS_EXPONENT) == BITS_EXPONENT && (_bits & BITS_SIGNIFICAND) == 0;
            }
        }

        public bool IsNan
        {
            get
            {
                return (_bits & BITS_EXPONENT) == BITS_EXPONENT && (_bits & BITS_SIGNIFICAND) != 0;
            }
        }

        public Binary64 NextRepresentableValue(Binary64InsignificantBits insignificantBits)
        {
            ulong lsb = (ulong)1 << (int)insignificantBits;
            ulong mask = lsb - 1;
            ulong maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            ulong nextBits = _bits & maxMask;

            if(Sign.IsNegative)
            {
                if(nextBits != 0)
                {
                    // Some negative number getting smaller.
                    nextBits -= lsb;
                    nextBits |= BITS_SIGN;
                }
                else
                {
                    // Negative 0 will turn into positive 0.
                }
            }
            else
            {
                if(nextBits == maxMask)
                {
                    throw new InvalidOperationException();
                }

                // Some positive number getting larger.
                nextBits += lsb;
            }

            return new Binary64(nextBits);
        }

        public Binary64 PreviousRepresentableValue(Binary64InsignificantBits insignificantBits)
        {
            ulong lsb = (ulong)1 << (int)insignificantBits;
            ulong mask = lsb - 1;
            ulong maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            ulong prevBits = _bits & maxMask;

            if (Sign.IsPositive)
            {
                if (prevBits != 0)
                {
                    // Some positive number getting smaller
                    prevBits -= lsb;
                }
                else
                {
                    // Positive 0 will turn into negative 0.
                    prevBits |= BITS_SIGN;
                }
            }
            else
            {
                if (prevBits == maxMask)
                {
                    throw new InvalidOperationException();
                }

                // Some negative number getting larger.
                prevBits -= lsb;
                prevBits |= BITS_SIGN;
            }

            return new Binary64(prevBits);
        }

        public Binary64 Round(Binary64InsignificantBits insignificantBits)
        {
            // Don't round special values...
            if (IsNan || IsInfinite)
            {
                return new Binary64(this._bits);
            }

            ulong lsb = (ulong)1 << (int)insignificantBits;
            ulong mask = lsb - 1;
            ulong maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            ulong bits = _bits & maxMask;
            
            // Imlpement rounding by adding half of the maximum value that can
            // be represented by the insignificant bits, and then truncating.
            bits += (mask / 2);

            // It is possible that the significand just overflowed.  If so, we
            // just rounded up to one of the special values (Infinity or Nan).
            // Simply return +/- infinite.
            if (bits > BITS_MAX_NORMAL)
            {
                if (Sign.IsNegative)
                {
                    // NegativeInfinity
                    bits = BITS_SIGN | BITS_EXPONENT;
                }
                else
                {
                    // PositiveInfinity
                    bits = BITS_EXPONENT;
                }
            }
            else
            {
                // Restore the sign.  We never round from negative to positive.
                if (Sign.IsNegative)
                {
                    bits |= BITS_SIGN;
                }
            }

            return new Binary64(bits);
        }

        private const ulong BITS_SIGNIFICAND =  0x000FFFFFFFFFFFFF;
        private const ulong BITS_EXPONENT =     0x7FF0000000000000;
        private const ulong BITS_SIGN =         0x8000000000000000;
        private const ulong BITS_MAX_POSITIVE = 0x7FFFFFFFFFFFFFFF;
        private const ulong BITS_MAX_NORMAL =   0x7FEFFFFFFFFFFFFF;

        [FieldOffset(0)]
        private ulong _bits;

        [FieldOffset(0)]
        private double _value;
    }
}
