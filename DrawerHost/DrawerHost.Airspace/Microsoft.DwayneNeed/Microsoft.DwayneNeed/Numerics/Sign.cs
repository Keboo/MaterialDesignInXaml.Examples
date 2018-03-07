using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Numerics
{
    /// <summary>
    ///     A simple type representing the sign of a numeric.
    /// </summary>
    public struct Sign
    {
        public Sign(bool isNegative)
        {
            _isNegative = isNegative;
        }

        public bool IsPositive { get { return !_isNegative; } }
        public bool IsNegative { get { return _isNegative; } }

        public static readonly Sign Positive = new Sign(false);
        public static readonly Sign Negative = new Sign(true); 

        private bool _isNegative;
    }
}
