using Microsoft.DwayneNeed.Reactive.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive
{
    /// <summary>
    /// ReadOnlyValue exposes only the IObservableValue interface.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public class ReadOnlyValue<T> : IObservableValue<T>
    {
        private IObservableValue<T> value;

        public ReadOnlyValue(IObservableValue<T> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

             this.value = value;
        }

        public T GetValue()
        {
            return this.value.GetValue();
        }

        public IObservable<T> Changed
        {
            get
            {
                return this.value.Changed;
            }
        }
    }
}
