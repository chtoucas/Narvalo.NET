namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;
    using Xunit;

    public static class MaybeFacts
    {
        public static class TheWhereOperator
        {
            [Fact]
            public static void ReturnsNone_ForNone()
            {
                // Arrange
                Maybe<int> q = from _ in Maybe.Create(1) where _ == 2 select _;

                // Act & Assert
                Assert.True(q.IsNone);
            }
        }

        public static class TheSelectOperator
        {
            [Fact]
            public static void ThrowsArgumentNullException_WhenSourceIsNull()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => from _ in (Maybe<int>)null select _);
            }

            [Fact]
            public static void ReturnsNone_ForNone()
            {
                // Arrange
                Maybe<int> q = from _ in Maybe<int>.None select _;

                // Act & Assert
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsSome_ForSome()
            {
                // Arrange
                Maybe<int> q = from _ in Maybe.Create(1) select _;

                // Act & Assert
                Assert.True(q.IsSome);
            }
        }
    }
}
