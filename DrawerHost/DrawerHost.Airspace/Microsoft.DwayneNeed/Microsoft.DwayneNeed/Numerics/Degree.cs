using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    // Used to preserve unit type-safety for a double-precision angle in degrees.
    public struct Degree
    {
        public Degree(double value)
        {
            _value = value;
        }

        public static Degree FromRadian(double radian)
        {
            return (Degree)new Radian(radian);
        }

        public static implicit operator double(Degree value)
        {
            return value._value;
        }

        public static implicit operator Degree(double value)
        {
            return new Degree(value);
        }

        public static explicit operator Radian(Degree value)
        {
            return new Radian((value._value * Math.PI) / 180.0);
        }

        public static Degree operator /(Degree a, double b)
        {
            return new Degree(a._value / b);
        }

        public static Degree operator *(Degree a, double b)
        {
            return new Degree(a._value * b);
        }

        public static Degree operator +(Degree a, Degree b)
        {
            return new Degree(a._value + b._value);
        }

        public static Degree operator -(Degree a, Degree b)
        {
            return new Degree(a._value - b._value);
        }

        private double _value;
    }
}
