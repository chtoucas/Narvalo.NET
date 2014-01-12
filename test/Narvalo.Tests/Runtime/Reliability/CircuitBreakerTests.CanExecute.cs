namespace Narvalo.Runtime.Reliability
{
    using Xunit;

    public partial class CircuitBreakerTests
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
