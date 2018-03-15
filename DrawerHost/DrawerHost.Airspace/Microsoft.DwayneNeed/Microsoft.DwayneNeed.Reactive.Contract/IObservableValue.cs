using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive.Contract
{
    /// <summary>
    /// Represents a value that can be observed.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public interface IObservableValue<out T>
    {
        /// <summary>
        /// Returns the current value.
        /// </summary>
        T GetValue();

        /// <summary>
        /// Presents a sequence of values as the value changes.
        /// </summary>
        /// <remarks>
        /// When an observer subscribes, they will be notified of the current
        /// value.  The sequence may complete if the value is guaranteed not
        /// to change.
        /// </remarks>
        IObservable<T> Changed { get; }
    }
}
