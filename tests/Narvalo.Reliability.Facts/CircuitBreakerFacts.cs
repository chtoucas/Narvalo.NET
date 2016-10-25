// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    using Xunit;

    // Method injection
    // http://www.codeproject.com/Articles/37549/CLR-Injection-Runtime-Method-Replacer
    // http://blog.stevensanderson.com/2009/08/24/writing-great-unit-tests-best-and-worst-practises/

    public static partial class CircuitBreakerFacts
    {
        private static CircuitBreaker CreateCircuitBreaker()
        {
            return new CircuitBreaker(1 /* threshold */, TimeSpan.Zero);
        }

        //private static CircuitBreaker CreateCircuitBreaker(int threshold)
        //{
        //    return new CircuitBreaker(threshold, TimeSpan.MaxValue);
        //}

        [Serializable]
        public class RetryableException : Exception
        {
            public RetryableException() : base() { }

            public RetryableException(string message) : base(message) { }

            public RetryableException(string message, Exception innerException)
                : base(message, innerException) { }

            protected RetryableException(SerializationInfo info, StreamingContext context)
                : base(info, context) { }
        }
    }

    public static partial class CircuitBreakerFacts
    {
        #region CanInvoke

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

        [Fact]
        public static void CanInvoke_ReturnsTrue_WhenClosed()
        {
            // Arrange
            var cb = CreateCircuitBreaker();

            // Act
            cb.Close();

            // Assert
            Assert.True(cb.CanInvoke);
        }

        [Fact]
        public static void CanInvoke_ReturnsTrue_WhenHalfOpen()
        {
            // Arrange
            var cb = CreateCircuitBreaker();

            // Act
            cb.HalfOpen();

            // Assert
            Assert.True(cb.CanInvoke);
        }

        [Fact]
        public static void CanInvoke_ReturnsFalse_WhenOpen()
        {
            // Arrange
            var cb = CreateCircuitBreaker();

            // Act
            cb.Trip();

            // Assert
            Assert.False(cb.CanInvoke);
        }

#endif

        #endregion

        #region Invoke()

        #region Closed circuit

        [Fact]
        public static void Invoke_WhenClosed_InvokesOperation()
        {
            // Arrange
            var cb = CreateCircuitBreaker();
            bool wasCalled = false;
            Action op = () => { wasCalled = true; };

            // Act
            cb.Invoke(op);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public static void Invoke_AssignsCapturedVariable_WhenClosed()
        {
            // Arrange
            var cb = CreateCircuitBreaker();
            int expectedValue = 1;
            int? result = null;
            Action op = () => { result = expectedValue; };

            // Act
            cb.Invoke(op);

            // Assert
            Assert.True(result.HasValue && result.Value == expectedValue);
        }

        [Fact]
        public static void Invoke_DoesNotChangeState_ForSuccessfulAction_WhenClosed()
        {
            // Arrange
            var cb = CreateCircuitBreaker();
            Action op = () => { };

            // Act
            cb.Invoke(op);

            // Assert
            Assert.True(cb.IsClosed);
        }

        [Fact]
        public static void Invoke_Rethrows_ForFaultyAction_WhenClosed()
        {
            // Arrange
            var cb = CreateCircuitBreaker();
            Action op = () => { throw new RetryableException(); };

            // Act & Assert
            Assert.Throws<RetryableException>(() => { cb.Invoke(op); });
        }

        #endregion

        #region Open circuit

        //[Fact]
        //public static void Invoke_WhenOpen_DoesNotInvokeOperation()
        //{
        //    // Arrange
        //    var cb = NewCircuitBreaker();
        //    cb.Open();
        //    bool wasCalled = false;
        //    Action op = () => { wasCalled = true; };

        //    // Act
        //    try { cb.Invoke(op); }
        //    catch { }

        //    // Assert
        //    Assert.False(wasCalled);
        //}

        //[Fact]
        //public static void Invoke_WhenOpen_ThrowsInvalidOperationException()
        //{
        //    // Arrange
        //    var cb = NewCircuitBreaker();
        //    cb.Open();
        //    Action op = () => { };

        //    // Act & Assert
        //    Assert.Throws<InvalidOperationException>(() => { cb.Invoke(op); });
        //}

        #endregion

        //[Fact]
        //[TestCase("", Result = 100d)]
        //[TestCase("bad", Result = 80d)]
        //[TestCase("bad good", Result = 100d)]
        //[TestCase("bad bad", Result = 60d)]
        //[TestCase("bad bad good", Result = 80d)]
        //[TestCase("bad bad good good", Result = 100d)]
        //[TestCase("bad good bad good", Result = 100d)]
        //public static double ServiceLevel(string callPattern) {
        //    var cb = new CircuitBreaker(5, TimeSpan.FromMilliseconds(60));

        //    foreach (var call in callPattern.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
        //        switch (call) {
        //            case "good":
        //                cb.Execute(() => { });
        //                break;
        //            case "bad":
        //                try { cb.Invoke(() => { throw new Exception(); }); }
        //                catch (Exception) { }
        //                break;
        //            default:
        //                Assert.Fail("Unknown call sequence.");
        //                break;
        //        }
        //    }

        //    return cb.CurrentServiceLevel;
        //}

        #endregion
    }
}
