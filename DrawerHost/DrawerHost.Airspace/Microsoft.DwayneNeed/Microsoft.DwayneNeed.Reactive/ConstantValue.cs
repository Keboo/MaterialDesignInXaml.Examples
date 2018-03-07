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
    /// ConstantValue implements the IObservableValue interface for a value
    /// that cannot change.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public class ConstantValue<T> : IObservableValue<T>
    {
        private T value;
        private IObservable<T> changed;

        public ConstantValue(T value)
        {
            this.value = value;
            this.changed = Observable.Return<T>(this.value);
        }

        public T GetValue()
        {
            return this.value;
        }

        public IObservable<T> Changed
        {
            get
            {
                return this.changed;
            }
        }
    }
}
