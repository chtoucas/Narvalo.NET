// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
#if !NO_INTERNALS_VISIBLE_TO

    using System;

    using Xunit;

    public static class CountryISOCodesFacts
    {
        #region TwoLetterCodeExists()

        [Fact]
        public static void TwoLetterCodeExists_ReturnsFalse_ForNull()
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(null));

        [Fact]
        public static void TwoLetterCodeExists_ReturnsFalse_ForEmptyString()
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(String.Empty));

        [Theory]
        [InlineData("A")]
        [InlineData("ABC")]
        [InlineData("ABCD")]
        public static void TwoLetterCodeExists_ReturnsFalse_ForInvalidLength(string value)
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(value));

        [Fact]
        public static void TwoLetterCodeExists_ReturnsTrue_ForValidCode()
            => Assert.True(CountryISOCodes.TwoLetterCodeExists("FR"));

        #endregion
    }

#endif
}
