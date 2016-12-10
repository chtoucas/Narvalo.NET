// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class EnforceFacts
    {
        #region NotWhiteSpace()

        [Fact]
        public static void NotWhiteSpace_ThrowsArgumentException_ForWhiteSpaceOnlyString()
            => Assert.Throws<ArgumentException>(() => Enforce.NotWhiteSpace(My.WhiteSpaceOnlyString, "paramName"));

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNull()
            => Enforce.NotWhiteSpace(null, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForEmptyString()
            => Enforce.NotWhiteSpace(String.Empty, "paramName");

        [Fact]
        public static void NotWhiteSpace_DoesNotThrow_ForNonWhiteSpaceString()
            => Enforce.NotWhiteSpace("Whatever", "paramName");

        #endregion

        #region IsWhiteSpace()

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNull()
                => Assert.False(Enforce.IsWhiteSpace(null));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForEmptyString()
            => Assert.False(Enforce.IsWhiteSpace(String.Empty));

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNonWhiteSpaceOnlyString()
            => Assert.False(Enforce.IsWhiteSpace("Whatever"));

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
            => Assert.True(Enforce.IsWhiteSpace(My.WhiteSpaceOnlyString));

        #endregion
    }
}
