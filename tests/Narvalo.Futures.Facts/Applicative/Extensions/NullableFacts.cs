// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative.Extensions
{
    using System;

    using Xunit;

    public static class NullableFacts
    {
        #region Where()

        [Fact]
        public static void Where_ReturnsValue_ForSuccessfulPredicate()
        {
            // Arrange
            int? source = 1;
            Func<int, bool> predicate = _ => _ == 1;

            // Act
            var m = source.Where(predicate);
            var q = from _ in source where predicate(_) select _;

            // Assert
            Assert.True(m.HasValue);
            Assert.True(q.HasValue);
            Assert.Equal(1, m.Value);
            Assert.Equal(1, q.Value);
        }

        [Fact]
        public static void Where_ReturnsNull_ForFailedPredicate()
        {
            // Arrange
            int? source = 1;
            Func<int, bool> predicate = _ => _ == 2;

            // Act
            var m = source.Where(predicate);
            var q = from _ in source where predicate(_) select _;

            // Assert
            Assert.False(m.HasValue);
            Assert.False(q.HasValue);
        }

        #endregion

        #region SelectMany()

        [Fact]
        public static void SelectMany_ReturnsNull_WhenNull()
        {
            // Arrange
            int? source = null;
            int? middle = 2;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.HasValue);
            Assert.False(q.HasValue);
        }

        [Fact]
        public static void SelectMany_ReturnsNull_ForNullMiddle_WhenNull()
        {
            // Arrange
            int? source = null;
            int? middle = null;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.HasValue);
            Assert.False(q.HasValue);
        }

        [Fact]
        public static void SelectMany_ReturnsNull_ForNullMiddle()
        {
            // Arrange
            int? source = 1;
            int? middle = null;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.HasValue);
            Assert.False(q.HasValue);
        }

        [Fact]
        public static void SelectMany_ReturnsValueAfterApplyingSelector()
        {
            // Arrange
            int? source = 1;
            int? middle = 2;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.True(m.HasValue);
            Assert.True(q.HasValue);
            Assert.Equal(3, m.Value);
            Assert.Equal(3, q.Value);
        }

        #endregion
    }
}
