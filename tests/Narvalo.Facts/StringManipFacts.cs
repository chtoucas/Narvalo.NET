// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class StringManipFacts
    {
        #region Reverse()

        [Fact]
        public static void Reverse_ThrowsArgumentNullException_ForNullString()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { StringManip.Reverse(null); });
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("ABCD", "DCBA")]
        [InlineData("éçàè$£ö", "ö£$èàçé")]
        [CLSCompliant(false)]
        public static void Reverse_ReturnsExpectedString(string value, string expectedValue)
        {
            // Act & Assert
            Assert.Equal(expectedValue, StringManip.Reverse(value));
        }

        #endregion
    }
}
