namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Extensions;

    public partial class CircuitBreakerTests
    {
        public static IEnumerable<object[]> MaxRetriesUnderThreshold
        {
            get
            {
                yield return new object[] { 5, 1 };
                yield return new object[] { 5, 2 };
                yield return new object[] { 5, 3 };
                yield return new object[] { 5, 4 };
            }
        }

        public static IEnumerable<object[]> MaxRetriesOverOrEqualToThreshold
        {
            get
            {
                yield return new object[] { 5, 5 };
                yield return new object[] { 5, 6 };
                yield return new object[] { 5, 7 };
                yield return new object[] { 5, 8 };
            }
        }

        #region Closed circuit

        [Fact]
        [PropertyData("MaxRetriesLessThanThreshold")]
        public void Execute_FaultyAction_WhenClosedAndMaxRetriesUnderThreshold_InvokesActionMaxRetriesPlusOneTimes(
            int threshold, int maxRetries)
        {

            // Arrange
            var cb = NewCircuitBreaker(threshold);
            var policy = NewRetryPolicy(maxRetries);
            int count = 0;
            Action op = () => { count++; throw new RetryableException(); };

            // Act
            try { cb.Execute(op, policy); }
            catch { }

            // Assert
            Assert.Equal(maxRetries + 1, count);
        }

        [Fact]
        [PropertyData("MaxRetriesOverOrEqualToThreshold")]
        public void Execute_FaultyAction_WhenClosedAndMaxRetriesOverOrEqualToThreshold_InvokesActionOnlyThresholdTimes(
            int threshold, int maxRetries)
        {

            // Arrange
            var cb = NewCircuitBreaker(threshold);
            var policy = NewRetryPolicy(maxRetries);
            int count = 0;
            Action op = () => { count++; throw new RetryableException(); };

            // Act
            try { cb.Execute(op, policy); }
            catch { }

            // Assert
            Assert.Equal(threshold, count);
        }

        [Fact]
        [PropertyData("MaxRetriesUnderThreshold")]
        public void Execute_FaultyAction_WhenClosedAndMaxRetriesUnderThreshold_DoesNotChangeState(
            int threshold, int maxRetries)
        {

            // Arrange
            var cb = NewCircuitBreaker(threshold);
            var policy = NewRetryPolicy(maxRetries);
            Action op = () => { throw new RetryableException(); };

            // Act
            try { cb.Execute(op, policy); }
            catch { }

            // Assert
            Assert.True(cb.IsClosed);
        }

        [Fact]
        [PropertyData("MaxRetriesOverOrEqualToThreshold")]
        public void Execute_FaultyAction_WhenClosedAndMaxRetriesOverOrEqualToThreshold_OpenCircuit(
            int threshold, int maxRetries)
        {

            // Arrange
            var cb = NewCircuitBreaker(threshold);
            var policy = NewRetryPolicy(maxRetries);
            Action op = () => { throw new RetryableException(); };

            // Act
            try { cb.Execute(op, policy); }
            catch { }

            // Assert
            Assert.True(cb.IsOpen);
        }

        [Fact]
        public void Execute_FaultyAction_WhenClosed_ThrowsAggregateException()
        {
            // Arrange
            var cb = NewCircuitBreaker(5);
            var policy = NewRetryPolicy(4);
            int count = 0;
            Action op = () => { count++; throw new RetryableException(count.ToString()); };

            // Act & Assert
            var ex = Assert.Throws<AggregateException>(() => { cb.Execute(op, policy); });
            int index = 0;
            Assert.True(
                ex.InnerExceptions.Count == 5
                && ex.InnerExceptions.All((e) => { return e.Message == (++index).ToString(); })
            );
        }

        #endregion
    }
}


