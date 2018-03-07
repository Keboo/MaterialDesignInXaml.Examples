using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DwayneNeed.Media
{
    public class CacheScale
    {
        public static CacheScale Auto
        {
            get
            {
                if (_auto == null)
                {
                    _auto = new CacheScale(null);
                }

                return _auto;
            }
        }

        public CacheScale(double scale)
            : this((double?)scale)
        {
        }

        public bool IsAuto
        {
            get
            {
                return !_scale.HasValue;
            }
        }

        public double Scale
        {
            get
            {
                return _scale.Value;
            }
        }

        private CacheScale(double? scale)
        {
            _scale = scale;
        }

        private static CacheScale _auto;
        private double? _scale;
    }
}
