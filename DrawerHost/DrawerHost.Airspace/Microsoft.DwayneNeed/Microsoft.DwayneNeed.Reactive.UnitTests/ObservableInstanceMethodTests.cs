using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Microsoft.DwayneNeed.Reactive.UnitTests
{
    [TestClass]
    public class ObservableInstanceMethodTests
    {
        [TestMethod]
        public void ObservableInstanceMethodConstructorShouldThrowIfANullInstanceIsPassed()
        {
            MethodInfo methodInfo = typeof(ExampleClass).GetMethod(
                "PublicInstanceMethodNoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static); 
            
            Action action = () => new ObservableInstanceMethod<ExampleClass,Int32>(null, methodInfo);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ObservableInstanceMethodConstructorShouldThrowIfANullMethodInfoIsPassed()
        {
            var instance = new ExampleClass(0);

            Action action = () => new ObservableInstanceMethod<ExampleClass, Int32>(instance, null);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ObservableInstanceMethodConstructorShouldThrowIfAStaticMethodInfoIsPassed()
        {
            var methodInfo = typeof(ExampleClass).GetMethod(
                "PublicStaticMethodNoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var instance = new ExampleClass(0);

            Action action = () => new ObservableInstanceMethod<ExampleClass, Int32>(instance, methodInfo);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ObservableInstanceMethodConstructorShouldThrowIfAMethodInfoIsPassedThatTakesParameters()
        {
            var methodInfo = typeof(ExampleClass).GetMethod(
                "PublicInstanceMethodTwoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var instance = new ExampleClass(0);

            Action action = () => new ObservableInstanceMethod<ExampleClass, Int32>(instance, methodInfo);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ObservableInstanceMethodConstructorShouldInvokeMethodOnInstance()
        {
            var methodInfo = typeof(ExampleClass).GetMethod(
                "PublicInstanceMethodNoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var instance = new ExampleClass(10);
            var observableInstanceMethod = new ObservableInstanceMethod<ExampleClass, Int32>(instance, methodInfo);

            observableInstanceMethod.Result.GetValue().Should().Be(10);
            instance.GetCallCount(methodInfo.Name).Should().Be(1);
        }

        [TestMethod]
        public void ObservableInstanceMethodShouldInvokeMethodWhenInstanceChangesAndRaiseChangedEvent()
        {
            var methodInfo = typeof(ExampleClass).GetMethod(
                "PublicInstanceMethodNoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var instance = new ExampleClass(10);
            var changes = new List<int>();
            var observableInstanceMethod = new ObservableInstanceMethod<ExampleClass, Int32>(instance, methodInfo);

            observableInstanceMethod.Result.Changed.Subscribe((v) => changes.Add(v));
            changes.Count.Should().Be(1);
            changes[0].Should().Be(10);

            observableInstanceMethod.Result.GetValue().Should().Be(10);
            instance.GetCallCount(methodInfo.Name).Should().Be(1);

            var instance2 = new ExampleClass(20);
            observableInstanceMethod.Instance.SetValue(instance2);
            changes.Count.Should().Be(2);
            changes[0].Should().Be(10);
            changes[1].Should().Be(20);
           
            observableInstanceMethod.Result.GetValue().Should().Be(20);
            instance.GetCallCount(methodInfo.Name).Should().Be(1);
            instance2.GetCallCount(methodInfo.Name).Should().Be(1);
        }

        [TestMethod]
        public void ObservableInstanceMethodShouldInvokeMethodButNotChangeWhenInstanceChangesButMethodReturnsTheSameValue()
        {
            var methodInfo = typeof(ExampleClass).GetMethod(
                "PublicInstanceMethodNoParamsReturnInt32",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var instance = new ExampleClass(10);
            var changes = new List<int>();
            var observableInstanceMethod = new ObservableInstanceMethod<ExampleClass, Int32>(instance, methodInfo);

            observableInstanceMethod.Result.Changed.Subscribe((v) => changes.Add(v));
            changes.Count.Should().Be(1);
            changes[0].Should().Be(10);

            observableInstanceMethod.Result.GetValue().Should().Be(10);
            instance.GetCallCount(methodInfo.Name).Should().Be(1);

            var instance2 = new ExampleClass(10);
            observableInstanceMethod.Instance.SetValue(instance2);
            changes.Count.Should().Be(1);
            changes[0].Should().Be(10);

            observableInstanceMethod.Result.GetValue().Should().Be(10);
            instance.GetCallCount(methodInfo.Name).Should().Be(1);
            instance2.GetCallCount(methodInfo.Name).Should().Be(1);
        }
    }
}
