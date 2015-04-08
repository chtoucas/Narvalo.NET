// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class AssumeFacts
    {
        #region AssumeNotNull()

        [Fact]
        public static void AssumeNotNull_ReturnsBackTheInput()
        {
            // Arrange
            var obj = new Object();

            // Act
            var result = obj.AssumeNotNull();

            // Assert
            Assert.True(ReferenceEquals(obj, result));
        }

        #endregion

        #region Invariant()

        [Fact]
        public static void Invariant_DoesNothing()
        {
            // Arrange
            var obj = new Object();

            // Act
            Assume.Invariant(obj);
        }

        #endregion
    }
}
