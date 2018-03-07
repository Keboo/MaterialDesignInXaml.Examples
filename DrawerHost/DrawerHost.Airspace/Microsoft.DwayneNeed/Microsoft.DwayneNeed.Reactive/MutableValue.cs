using Microsoft.DwayneNeed.Reactive.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive
{
    /// <summary>
    /// MutableValue implements the IMutableValue interface to provide a value
    /// that can read and written, and changes to the value can be observed.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public class MutableValue<T> : IMutableValue<T>
    {
        private BehaviorSubject<T> value;
        private IObservable<T> changed;
        private Func<T, bool> validateValue;

        public MutableValue(T value = default(T), Func<T,bool> validateValue = null)
        {
            // Ensure the initial value is acceped by the validateSetter.
            ValidateValue(value, validateValue);

            this.value = new BehaviorSubject<T>(value);
            this.validateValue = validateValue;
            this.changed = Observable.Create((IObserver<T> observer) => this.value.Subscribe(observer));
        }

        public T GetValue()
        {
            return this.value.Value;
        }

        public void SetValue(T value)
        {
            dynamic dynamicValue = value;
            if (dynamicValue != this.value.Value)
            {
                this.ValidateValue(value);
                this.value.OnNext(value);
            }
        }

        public IObservable<T> Changed
        {
            get
            {
                return this.changed;
            }
        }

        private void ValidateValue(T value)
        {
            ValidateValue(value, this.validateValue);
        }

        private static void ValidateValue(T value, Func<T, bool> validateValue)
        {
            bool isValid = validateValue != null ? validateValue(value) : true;
            if(!isValid)
            {
                throw new InvalidOperationException("Invalid value");
            }
        }
    }
}
