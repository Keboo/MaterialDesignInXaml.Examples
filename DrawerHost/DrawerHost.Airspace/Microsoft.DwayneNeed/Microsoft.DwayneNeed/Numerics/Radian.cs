using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    // Used to preserve unit type-safety for a double-precision angle in radians.
    public struct Radian
    {
        public Radian(double value)
        {
            _value = value;
        }

        public static Radian FromDegree(double degree)
        {
            return (Radian) new Degree(degree);
        }

        public static implicit operator double(Radian value)
        {
            return value._value;
        }

        public static implicit operator Radian(double value)
        {
            return new Radian(value);
        }

        public static explicit operator Degree(Radian value)
        {
            return new Degree((value._value * 180.0) / Math.PI);
        }

        public static Radian operator /(Radian a, double b)
        {
            return new Radian(a._value / b);
        }

        public static Radian operator *(Radian a, double b)
        {
            return new Radian(a._value * b);
        }

        public static Radian operator +(Radian a, Radian b)
        {
            return new Radian(a._value + b._value);
        }

        public static Radian operator -(Radian a, Radian b)
        {
            return new Radian(a._value - b._value);
        }

        private double _value;
    }
}
