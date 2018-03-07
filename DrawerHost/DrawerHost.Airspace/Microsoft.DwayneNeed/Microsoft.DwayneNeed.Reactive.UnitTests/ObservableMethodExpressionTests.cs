using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    [TestClass]
    public class ObservableMethodExpressionTests
    {
        [TestMethod]
        public void TryItOut()
        {
            var sampleClass = new SampleClass(10);
            var observableMethodExpression = new ObservableExpression<int>(() => sampleClass.Method());
        }

        private class SampleClass
        {
            public SampleClass(int value)
            {
                this.Field = value;
            }

            public int Field;

            public int Property
            {
                get
                {
                    return this.Field;
                }

                set
                {
                    this.Field = value;
                }
            }

            public int Method()
            {
                return this.Property;
            }

            public void Method(int value)
            {
                this.Property = value;
            }
        }
    }
}
