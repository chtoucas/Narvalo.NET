// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using Xunit;

    public partial class CircuitBreakerFacts
    {
        #region Closed circuit

        [Fact]
        public void Execute_WhenClosed_InvokesOperation()
        {
            // Arrange
            var cb = NewCircuitBreaker();
            bool wasCalled = false;
            Action op = () => { wasCalled = true; };

            // Act
            cb.Execute(op);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public void Execute_ActionWithCapturedVariable_WhenClosed_AssignsCapturedVariable()
        {
            // Arrange
            var cb = NewCircuitBreaker();
            int expectedValue = 1;
            int? result = null;
            Action op = () => { result = expectedValue; };

            // Act
            cb.Execute(op);

            // Assert
            Assert.True(result.HasValue && result.Value == expectedValue);
        }

        [Fact]
        public void Execute_SuccessfulAction_WhenClosed_DoesNotChangeState()
        {
            // Arrange
            var cb = NewCircuitBreaker();
            Action op = () => { };

            // Act
            cb.Execute(op);

            // Assert
            Assert.True(cb.IsClosed);
        }

        [Fact]
        public void Execute_FaultyAction_WhenClosed_Rethrows()
        {
            // Arrange
            var cb = NewCircuitBreaker();
            Action op = () => { throw new RetryableException(); };

            // Act & Assert
            Assert.Throws<RetryableException>(() => { cb.Execute(op); });
        }

        #endregion

        #region Open circuit

        //[Fact]
        //public void Execute_WhenOpen_DoesNotInvokeOperation()
        //{
        //    // Arrange
        //    var cb = NewCircuitBreaker();
        //    cb.Open();
        //    bool wasCalled = false;
        //    Action op = () => { wasCalled = true; };

        //    // Act
        //    try { cb.Execute(op); }
        //    catch { }

        //    // Assert
        //    Assert.False(wasCalled);
        //}

        //[Fact]
        //public void Execute_WhenOpen_ThrowsInvalidOperationException()
        //{
        //    // Arrange
        //    var cb = NewCircuitBreaker();
        //    cb.Open();
        //    Action op = () => { };

        //    // Act & Assert
        //    Assert.Throws<InvalidOperationException>(() => { cb.Execute(op); });
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
        //public double ServiceLevel(string callPattern) {
        //    var cb = new CircuitBreaker(5, TimeSpan.FromMilliseconds(60));

        //    foreach (var call in callPattern.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
        //        switch (call) {
        //            case "good":
        //                cb.Execute(() => { });
        //                break;
        //            case "bad":
        //                try { cb.Execute(() => { throw new Exception(); }); }
        //                catch (Exception) { }
        //                break;
        //            default:
        //                Assert.Fail("Unknown call sequence.");
        //                break;
        //        }
        //    }

        //    return cb.CurrentServiceLevel;
        //}
    }
}
