using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.DwayneNeed.Reactive.Contract;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    [TestClass]
    public class ConstantValueTests
    {
        [TestMethod]
        public void ConstantValueShouldReturnValuePassedToConstructor()
        {
            var value = new ConstantValue<int>(10);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);

            value.GetValue().Should().Be(10);
            changedValues.Count.Should().Be(1);
            changedValues[0].Should().Be(10);
            changedCompleted.Should().BeTrue();
        }
    }
}
