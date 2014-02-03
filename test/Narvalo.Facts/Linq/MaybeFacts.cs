﻿namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;
    using Xunit;

    public static class MaybeFacts
    {
        public static class TheWhereOperator
        {
            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                var source = Maybe.Create(1);

                // Act
                var m = source.Where(_ => _ == 1);
                var q = from _ in source where _ == 1 select _;

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
                Assert.Equal(m.Value, 1);
                Assert.Equal(q.Value, 1);
            }

            [Fact]
            public static void ReturnsNone_WhenNone()
            {
                // Arrange
                var source = Maybe.Create(1);

                // Act
                var m = source.Where(_ => _ == 2);
                var q = from _ in source where _ == 2 select _;

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }
        }

        public static class TheSelectOperator
        {
            [Fact]
            public static void ThrowsArgumentNullException_WhenSourceIsNull()
            {
                // Arrange
                var source = (Maybe<int>)null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => from _ in source select _);
            }

            [Fact]
            public static void ReturnsNone_WhenNone()
            {
                // Arrange
                var source = Maybe<int>.None;

                // Act
                var m = source.Select(_ => _);
                var q = from _ in source select _;

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                var source = Maybe.Create(1);

                // Act
                var m = source.Select(_ => _);
                var q = from _ in source select _;

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
            }
        }

        public static class TheSelectManyOperator
        {
            [Fact]
            public static void ReturnsSome_WhenSome()
            {
                // Arrange
                var source = Maybe.Create(1);

                // Act
                var m = source.SelectMany(_ => Maybe.Create(2 * _));

                // Assert
                Assert.True(m.IsSome);
                Assert.Equal(m.Value, 2);
            }

            [Fact]
            public static void ReturnsSome_WhenSomeX()
            {
                // Arrange
                var source = Maybe.Create(1);
                var middle = Maybe.Create(2);

                // Act
                var m = source.SelectMany(_ => middle, (i, j) => i + j);
                var q = from i in source
                        from j in middle
                        select i + j;

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
                Assert.Equal(m.Value, 3);
                Assert.Equal(q.Value, 3);
            }
        }
    }
}
