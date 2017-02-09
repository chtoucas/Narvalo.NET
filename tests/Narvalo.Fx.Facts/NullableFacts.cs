// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static class NullableFacts
    {
        #region Select()

        [Fact]
        public static void Select_ReturnsNull_WhenNull()
        {
            // Arrange
            int? source = null;
            Func<int, int> selector = _ => _;

            // Act
            var m = source.Select(selector);
            var q = from _ in source select selector(_);

            // Assert
            Assert.False(m.HasValue);
            Assert.False(q.HasValue);
        }

        [Fact]
        public static void Select_ReturnsValueAfterApplyingSelector()
        {
            // Arrange
            int? source = 1;
            Func<int, int> selector = _ => 2 * _;

            // Act
            var m = source.Select(selector);
            var q = from _ in source select selector(_);

            // Assert
            Assert.True(m.HasValue);
            Assert.True(q.HasValue);
            Assert.Equal(2, m.Value);
            Assert.Equal(2, q.Value);
        }

        #endregion
    }
}
