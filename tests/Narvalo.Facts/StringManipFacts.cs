// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static class StringManipFacts
    {
        public static IEnumerable<object[]> ReverseData
        {
            get
            {
                yield return new object[] { String.Empty, String.Empty };
                yield return new object[] { "ABCD", "DCBA" };
                yield return new object[] { "éçàè$£ö", "ö£$èàçé" };
            }
        }

        #region Reverse()

        [Fact]
        public static void Reverse_ThrowsArgumentNullException_ForNullString()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { StringManip.Reverse(null); });
        }

        [Theory]
        [MemberData("ReverseData")]
        [CLSCompliant(false)]
        public static void Reverse_ReturnsExpectedString(string expectedValue, string value)
        {
            // Act & Assert
            Assert.Equal(expectedValue, StringManip.Reverse(value));
        }

        #endregion
    }
}
