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
            public static void ThrowsArgumentNullException_WhenSourceIsNull()
            {
                // Arrange
                var source = (Maybe<int>)null;
                Func<int, bool> predicate = _ => _ == 1;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
                Assert.Throws<ArgumentNullException>(() => from _ in source where predicate(_) select _);
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullPredicate()
            {
                // Arrange
                var source = Maybe.Create(1);
                Func<int, bool> predicate = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
            }

            [Fact]
            public static void ReturnsSome_ForSuccessfulPredicate()
            {
                // Arrange
                var source = Maybe.Create(1);
                Func<int, bool> predicate = _ => _ == 1;

                // Act
                var m = source.Where(predicate);
                var q = from _ in source where predicate(_) select _;

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
                Assert.Equal(1, m.Value);
                Assert.Equal(1, q.Value);
            }

            [Fact]
            public static void ReturnsNone_ForUnsucessfulPredicate()
            {
                // Arrange
                var source = Maybe.Create(1);
                Func<int, bool> predicate = _ => _ == 2;

                // Act
                var m = source.Where(predicate);
                var q = from _ in source where predicate(_) select _;

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
                Func<int, int> selector = _ => _;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.Select(selector));
                Assert.Throws<ArgumentNullException>(() => from _ in source select selector(_));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullSelector()
            {
                // Arrange
                var source = Maybe.Create(1);
                Func<int, int> selector = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.Select(selector));
            }

            [Fact]
            public static void ReturnsNone_WhenSourceIsNone()
            {
                // Arrange
                var source = Maybe<int>.None;
                Func<int, int> selector = _ => _;

                // Act
                var m = source.Select(selector);
                var q = from _ in source select selector(_);

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsSomeAndApplySelector_WhenSourceIsSome()
            {
                // Arrange
                var source = Maybe.Create(1);
                Func<int, int> selector = _ => 2 * _;

                // Act
                var m = source.Select(selector);
                var q = from _ in source select selector(_);

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
                Assert.Equal(2, m.Value);
                Assert.Equal(2, q.Value);
            }
        }

        public static class TheSelectManyOperator
        {
            [Fact]
            public static void ThrowsArgumentNullException_WhenSourceIsNull()
            {
                // Arrange
                var source = (Maybe<int>)null;
                var middle = Maybe.Create(2);
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
                Assert.Throws<ArgumentNullException>(() => from i in source
                                                           from j in middle
                                                           select resultSelector(i, j));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullValueSelector()
            {
                // Arrange
                var source = Maybe.Create(1);
                var middle = Maybe.Create(2);
                Func<int, Maybe<int>> valueSelector = null;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullResultSelector()
            {
                // Arrange
                var source = Maybe.Create(1);
                var middle = Maybe.Create(2);
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
            }

            [Fact]
            public static void ReturnsNone_WhenSourceIsNone()
            {
                // Arrange
                var source = Maybe<int>.None;
                var middle = Maybe.Create(2);
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act
                var m = source.SelectMany(valueSelector, resultSelector);
                var q = from i in source
                        from j in middle
                        select resultSelector(i, j);

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsNone_ForMiddleIsNone()
            {
                // Arrange
                var source = Maybe.Create(1);
                var middle = Maybe<int>.None;
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act
                var m = source.SelectMany(valueSelector, resultSelector);
                var q = from i in source
                        from j in middle
                        select resultSelector(i, j);

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsNone_WhenSourceIsNone_ForMiddleIsNone()
            {
                // Arrange
                var source = Maybe<int>.None;
                var middle = Maybe<int>.None;
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act
                var m = source.SelectMany(valueSelector, resultSelector);
                var q = from i in source
                        from j in middle
                        select resultSelector(i, j);

                // Assert
                Assert.True(m.IsNone);
                Assert.True(q.IsNone);
            }

            [Fact]
            public static void ReturnsSomeAndApplySelector()
            {
                // Arrange
                var source = Maybe.Create(1);
                var middle = Maybe.Create(2);
                Func<int, Maybe<int>> valueSelector = _ => middle;
                Func<int, int, int> resultSelector = (i, j) => i + j;

                // Act
                var m = source.SelectMany(valueSelector, resultSelector);
                var q = from i in source
                        from j in middle
                        select resultSelector(i, j);

                // Assert
                Assert.True(m.IsSome);
                Assert.True(q.IsSome);
                Assert.Equal(3, m.Value);
                Assert.Equal(3, q.Value);
            }
        }

        //public static class TheJoinOperator
        //{
        //    [Fact]
        //    public static void ReturnsSome_WhenSomeX()
        //    {
        //        // Arrange
        //        var source = Maybe.Create(1);
        //        var inner = Maybe.Create(2);

        //        // Act
        //        var m = source.Join(inner, _ => _, _ => _, (i, j) => i + j);
        //        var q = from i in source
        //                join j in inner on i equals j
        //                select i + j;

        //        // Assert
        //        Assert.True(m.IsNone);
        //        Assert.True(q.IsNone);
        //    }

        //    [Fact]
        //    public static void ReturnsSome_WhenSome()
        //    {
        //        // Arrange
        //        var source = Maybe.Create(1);
        //        var inner = Maybe.Create(2);

        //        // Act
        //        var m = source.Join(inner, _ => 2 * _, _ => _, (i, j) => i + j);
        //        var q = from i in source
        //                join j in inner on 2 * i equals j
        //                select i + j;

        //        // Assert
        //        Assert.True(m.IsSome);
        //        Assert.True(q.IsSome);
        //        Assert.Equal(3, m.Value);
        //        Assert.Equal(3, q.Value);
        //    }
        //}
    }
}
