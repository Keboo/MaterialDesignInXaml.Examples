using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    public class ExampleClass
    {
        private Dictionary<string, int> callCounts = new Dictionary<string, int>();
        private int value;

        public ExampleClass(int value)
        {
            this.value = value;
        }

        public static void PublicStaticMethodNoParamsReturnVoid()
        {
        }

        public void PublicInstanceMethodNoParamsReturnVoid()
        {
            this.IncrementCallCount("PublicInstanceMethodNoParamsReturnVoid");
        }

        public void PublicInstanceMethodOneParamReturnVoid(string a)
        {
            this.IncrementCallCount("PublicInstanceMethodOneParamReturnVoid");
        }

        public void PublicInstanceMethodTwoParamsReturnVoid(int a, float b)
        {
            this.IncrementCallCount("PublicInstanceMethodTwoParamsReturnVoid");
        }

        public int PublicInstanceMethodNoParamsReturnInt32()
        {
            this.IncrementCallCount("PublicInstanceMethodNoParamsReturnInt32");
            return this.value;
        }

        public int PublicInstanceMethodOneParamReturnInt32(string a)
        {
            this.IncrementCallCount("PublicInstanceMethodOneParamReturnInt32");
            return this.value;
        }

        public int PublicInstanceMethodTwoParamsReturnInt32(int a, float b)
        {
            this.IncrementCallCount("PublicInstanceMethodTwoParamsReturnInt32");
            return this.value;
        }

        public int GetCallCount(string methodName)
        {
            if (this.callCounts.ContainsKey(methodName))
            {
                return this.callCounts[methodName];
            }
            else
            {
                return 0;
            }
        }

        private void IncrementCallCount(string methodName)
        {
            if (this.callCounts.ContainsKey(methodName))
            {
                this.callCounts[methodName] = this.callCounts[methodName] + 1;
            }
            else
            {
                this.callCounts[methodName] = 1;
            }
        }
    }
}
