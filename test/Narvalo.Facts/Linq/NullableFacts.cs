namespace Narvalo.Linq
{
    using Xunit;

    public static class NullableFacts
    {
        public static class TheWhereOperator
        {
            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                int? source = 1;

                // Act
                var m = source.Where(_ => _ == 1);
                var q = from _ in source where _ == 1 select _;

                // Assert
                Assert.True(m.HasValue);
                Assert.True(q.HasValue);
                Assert.Equal(m.Value, 1);
                Assert.Equal(q.Value, 1);
            }

            [Fact]
            public static void ReturnsNone_WhenNone()
            {
                // Arrange
                int? source = 1;

                // Act
                var m = source.Where(_ => _ == 2);
                var q = from _ in source where _ == 2 select _;

                // Assert
                Assert.False(m.HasValue);
                Assert.False(q.HasValue);
            }
        }

        public static class TheSelectOperator
        {
            [Fact]
            public static void ReturnsNone_WhenNone()
            {
                // Arrange
                int? source = null;

                // Act
                var m = source.Select(_ => _);
                var q = from _ in source select _;

                // Assert
                Assert.False(m.HasValue);
                Assert.False(q.HasValue);
            }

            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                int? source = 1;

                // Act
                var m = source.Select(_ => _);
                var q = from _ in source select _;

                // Assert
                Assert.True(m.HasValue);
                Assert.True(q.HasValue);
            }
        }

        public static class TheSelectManyOperator
        {
            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                int? source = 1;

                // Act
                var m = source.SelectMany(_ => (int?)(2 * _));

                // Assert
                Assert.True(m.HasValue);
                Assert.Equal(m.Value, 2);
            }

            [Fact]
            public static void ReturnsSome_WhenSomeX()
            {
                // Arrange
                int? source = 1;
                int? middle = 2;

                // Act
                var m = source.SelectMany(_ => middle, (i, j) => i + j);
                var q = from i in source
                        from j in middle
                        select i + j;

                // Assert
                Assert.True(m.HasValue);
                Assert.True(q.HasValue);
                Assert.Equal(m.Value, 3);
                Assert.Equal(q.Value, 3);
            }
        }
    }
}
