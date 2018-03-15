using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DwayneNeed.Reactive.Contract;

namespace Microsoft.DwayneNeed.Reactive
{
    public static class ValueExtensions
    {
        public static ReadOnlyValue<T> AsReadOnly<T>(this IObservableValue<T> value)
        {
            return new ReadOnlyValue<T>(value);
        }

        public static ConstantValue<T> AsConstant<T>(this IObservableValue<T> value)
        {
            return new ConstantValue<T>(value.GetValue());
        }
    }
}
