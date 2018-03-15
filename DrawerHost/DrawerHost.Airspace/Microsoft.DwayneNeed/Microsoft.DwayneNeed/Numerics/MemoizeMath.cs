using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    // A simple class that memoizes the results of the Math class.
    public class MemoizeMath
    {
        public MemoizeMath(double value)
        {
            Value = value;
        }

        public double Value {get; private set;}

        public double Abs
        {
            get
            {
                if (!_abs.HasValue)
                {
                    _abs = Math.Abs(Value);
                }

                return _abs.Value;
            }
        }
        double? _abs;

        public double ACos
        {
            get
            {
                if (!_acos.HasValue)
                {
                    _acos = Math.Acos(Value);
                }

                return _acos.Value;
            }
        }
        double? _acos;

        public double ASin
        {
            get
            {
                if (!_asin.HasValue)
                {
                    _asin = Math.Asin(Value);
                }

                return _asin.Value;
            }
        }
        double? _asin;

        public double ATan
        {
            get
            {
                if (!_atan.HasValue)
                {
                    _atan = Math.Atan(Value);
                }

                return _atan.Value;
            }
        }
        double? _atan;

        public double Ceiling
        {
            get
            {
                if (!_ceiling.HasValue)
                {
                    _ceiling = Math.Ceiling(Value);
                }

                return _ceiling.Value;
            }
        }
        double? _ceiling;

        public double Cos
        {
            get
            {
                if (!_cos.HasValue)
                {
                    _cos = Math.Cos(Value);
                }

                return _cos.Value;
            }
        }
        double? _cos;

        public double CosH
        {
            get
            {
                if (!_cosh.HasValue)
                {
                    _cosh = Math.Cosh(Value);
                }

                return _cosh.Value;
            }
        }
        double? _cosh;

        public double Exp
        {
            get
            {
                if (!_exp.HasValue)
                {
                    _exp = Math.Exp(Value);
                }

                return _exp.Value;
            }
        }
        double? _exp;

        public double Floor
        {
            get
            {
                if (!_floor.HasValue)
                {
                    _floor = Math.Floor(Value);
                }

                return _floor.Value;
            }
        }
        double? _floor;

        public double Log10
        {
            get
            {
                if (!_log10.HasValue)
                {
                    _log10 = Math.Log10(Value);
                }

                return _log10.Value;
            }
        }
        double? _log10;

        public int Sign
        {
            get
            {
                if (!_sign.HasValue)
                {
                    _sign = Math.Sign(Value);
                }

                return _sign.Value;
            }
        }
        int? _sign;

        public double Sin
        {
            get
            {
                if (!_sin.HasValue)
                {
                    _sin = Math.Sin(Value);
                }

                return _sin.Value;
            }
        }
        double? _sin;

        public double SinH
        {
            get
            {
                if (!_sinh.HasValue)
                {
                    _sinh = Math.Sinh(Value);
                }

                return _sinh.Value;
            }
        }
        double? _sinh;

        public double Sqrt
        {
            get
            {
                if (!_sqrt.HasValue)
                {
                    _sqrt = Math.Sqrt(Value);
                }

                return _sqrt.Value;
            }
        }
        double? _sqrt;

        public double Tan
        {
            get
            {
                if (!_tan.HasValue)
                {
                    _tan = Math.Tan(Value);
                }

                return _tan.Value;
            }
        }
        double? _tan;

        public double TanH
        {
            get
            {
                if (!_tanh.HasValue)
                {
                    _tanh = Math.Tanh(Value);
                }

                return _tanh.Value;
            }
        }
        double? _tanh;
    }
}
