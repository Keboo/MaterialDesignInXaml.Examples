using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive.Contract
{
    /// <summary>
    /// Represents a value that can be changed as well as observed.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public interface IMutableValue<T> : IObservableValue<T>
    {
        /// <summary>
        /// Sets the current value.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        void SetValue(T value);
    }
}
