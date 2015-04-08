// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static class VoidOrBreakFacts
    {
        #region IsBreak

        [Fact]
        public static void IsBreak_ReturnsFalse_WhenVoid()
        {
            // Act & Assert
            Assert.False(VoidOrBreak.Void.IsBreak);
        }

        [Fact]
        public static void IsBreak_ReturnsTrue_WhenBreak()
        {
            // Act & Assert
            Assert.True(VoidOrBreak.Break("reason").IsBreak);
        }

        #endregion

        #region Reason

        [Fact]
        public static void Reason_ThrowsInvalidOperationException_WhenVoid()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => VoidOrBreak.Void.Reason);
        }

        [Fact]
        public static void Reason_ReturnsOriginalReason_WhenBreak()
        {
            // Arrange
            var reason = "reason";

            // Act
            var vob = VoidOrBreak.Break(reason);

            // Assert
            Assert.Equal(reason, vob.Reason);
        }

        #endregion

        #region Break()

        [Fact]
        public static void Break_ThrowsArgumentNullException_ForNullMessage()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => VoidOrBreak.Break(null));
        }

        [Fact]
        public static void Break_ThrowsArgumentException_ForEmptyMessage()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => VoidOrBreak.Break(String.Empty));
        }

        #endregion

        #region ToString()

        [Fact]
        public static void ToString_ReturnsVoid_WhenVoid()
        {
            // Act & Assert
            Assert.Equal("Void", VoidOrBreak.Void.ToString());
        }

        [Fact]
        public static void ToString_ResultContainsReason_WhenBreak()
        {
            // Arrange
            var reason = "My reason to break.";

            // Act & Assert
            Assert.True(VoidOrBreak.Break(reason).ToString().Contains(reason));
        }

        #endregion
    }
}
