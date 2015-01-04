// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using Xunit;

    public partial class CircuitBreakerFacts
    {
        [Fact]
        public void WhenClosed_CanExecute_ReturnsTrue()
        {
            // Arrange
            var cb = NewCircuitBreaker();

            // Act
            cb.Close();

            // Assert
            Assert.True(cb.CanExecute);
        }

        [Fact]
        public void WhenHalfOpen_CanExecute_ReturnsTrue()
        {
            // Arrange
            var cb = NewCircuitBreaker();

            // Act
            cb.HalfOpen();

            // Assert
            Assert.True(cb.CanExecute);
        }

        //[Fact]
        //public void WhenOpen_CanExecute_ReturnsFalse()
        //{
        //    // Arrange
        //    var cb = NewCircuitBreaker();

        //    // Act
        //    cb.Open();

        //    // Assert
        //    Assert.False(cb.CanExecute);
        //}
    }
}
