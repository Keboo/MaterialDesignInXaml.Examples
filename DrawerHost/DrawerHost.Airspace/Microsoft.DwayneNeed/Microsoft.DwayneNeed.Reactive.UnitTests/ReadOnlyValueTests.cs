using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    [TestClass]
    public class ReadOnlyValueTests
    {
        [TestMethod]
        public void ReadOnlyValueShouldReturnValueOfOriginal()
        {
            var mutable = new MutableValue<int>(10);
            var value = new ReadOnlyValue<int>(mutable);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);

            value.GetValue().Should().Be(10);
            changedValues.Count.Should().Be(1);
            changedValues[0].Should().Be(10);
            changedCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void ReadOnlyValueShouldChangeIfOriginalChanges()
        {
            var mutable = new MutableValue<int>(10);
            var value = new ReadOnlyValue<int>(mutable);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);
            mutable.SetValue(20);

            value.GetValue().Should().Be(20);
            changedValues.Count.Should().Be(2);
            changedValues[0].Should().Be(10);
            changedValues[1].Should().Be(20);
            changedCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void ReadOnlyValueShouldNotExposeOriginal()
        {
            var mutable = new MutableValue<int>(10);
            var value = new ReadOnlyValue<int>(mutable);

            Object.ReferenceEquals(mutable, value.Changed).Should().BeFalse();
        }
    }
}
