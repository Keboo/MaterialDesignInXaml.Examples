using Microsoft.DwayneNeed.Reactive.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive
{
    public class ObservableInstanceMethod<TType,TResult>
    {
        private MethodInfo methodInfo;
        private MutableValue<TResult> result;

        public IMutableValue<TType> Instance { get; private set; }
        public IObservableValue<TResult> Result {get; private set;}

        public ObservableInstanceMethod(TType instance, MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            if (methodInfo.IsStatic)
            {
                throw new ArgumentException("Specified method is static.", "methodInfo");
            }

            if(!methodInfo.DeclaringType.IsAssignableFrom(typeof(TType)))
            {
                throw new ArgumentException("Method declaring type is not assignable from instance type.", "methodInfo");
            }

            if (!typeof(TResult).IsAssignableFrom(methodInfo.ReturnType))
            {
                throw new ArgumentException("Method return type is not assignable to result.", "methodInfo");
            }

            if (methodInfo.GetParameters().Length != 0)
            {
                throw new ArgumentException("Method takes parameters.", "methodInfo");
            }

            this.result = new MutableValue<TResult>();
            this.methodInfo = methodInfo;
            this.Instance = new MutableValue<TType>(instance, ValidateInstanceValue);
            this.Result = result.AsReadOnly();

            // Whenever the instance changes, we will refresh the result
            // by invoking the method.  Subscribing to this change will
            // immediately provide the current value, which will cause us
            // to immediately refresh.
            this.Instance.Changed.Subscribe(InstanceValueChanged);
        }

        private bool ValidateInstanceValue(TType instance)
        {
            return instance != null;
        }

        private void InstanceValueChanged(TType instance)
        {
            this.Refresh();
        }

        private void Refresh()
        {
            this.result.SetValue((TResult)this.methodInfo.Invoke(this.Instance.GetValue(), null));
        }
    }
}
