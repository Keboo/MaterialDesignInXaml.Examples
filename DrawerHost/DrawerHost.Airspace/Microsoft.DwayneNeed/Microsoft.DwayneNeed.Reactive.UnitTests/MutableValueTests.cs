using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Collections.Generic;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    [TestClass]
    public class MutableValueTests
    {
        [TestMethod]
        public void MutableValueConstructorShouldThrowIfValidateValueFails()
        {
            Action constructor = () => { new MutableValue<int>(10, v => v != 10); };
            constructor.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void MutableValueShouldReturnValuePassedToConstructor()
        {
            var value = new MutableValue<int>(10);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);

            value.GetValue().Should().Be(10);
            changedValues.Count.Should().Be(1);
            changedValues[0].Should().Be(10);
            changedCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void MutableValueShouldReturnValueWhenSet()
        {
            var value = new MutableValue<int>(10);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);
            value.SetValue(20);

            value.GetValue().Should().Be(20);
            changedValues.Count.Should().Be(2);
            changedValues[0].Should().Be(10);
            changedValues[1].Should().Be(20);
            changedCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void ObservableValueShouldNotChangeWhenSetToValueItAlreadyHas()
        {
            var value = new MutableValue<int>(10);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);
            value.SetValue(10);

            value.GetValue().Should().Be(10);
            changedValues.Count.Should().Be(1);
            changedValues[0].Should().Be(10);
            changedCompleted.Should().BeFalse();
        }

        [TestMethod]
        public void ObservableValueShouldNotChangeWhenSetToValueThatFailsValidation()
        {
            var value = new MutableValue<int>(10, v => v != 20);
            var changedValues = new List<int>();
            var changedCompleted = false;

            value.Changed.Subscribe((v) => changedValues.Add(v), () => changedCompleted = true);
            
            Action setter = () => value.SetValue(20);
            setter.ShouldThrow<InvalidOperationException>();

            value.GetValue().Should().Be(10);
            changedValues.Count.Should().Be(1);
            changedValues[0].Should().Be(10);
            changedCompleted.Should().BeFalse();
        }
    }
}
