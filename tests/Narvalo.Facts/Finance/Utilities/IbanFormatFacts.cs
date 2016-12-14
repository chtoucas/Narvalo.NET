// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class IbanFormatFacts
    {
        #region CheckBban()

        [Theory]
        [InlineData("1234567890")]
        [InlineData("12345678901")]
        [InlineData("123456789012")]
        [InlineData("1234567890123")]
        [InlineData("12345678901234")]
        [InlineData("123456789012345")]
        [InlineData("1234567890123456")]
        [InlineData("12345678901234567")]
        [InlineData("123456789012345678")]
        [InlineData("1234567890123456789")]
        [InlineData("12345678901234567890")]
        [InlineData("123456789012345678901")]
        [InlineData("1234567890123456789012")]
        [InlineData("12345678901234567890123")]
        [InlineData("123456789012345678901234")]
        [InlineData("1234567890123456789012345")]
        [InlineData("12345678901234567890123456")]
        [InlineData("123456789012345678901234567")]
        [InlineData("1234567890123456789012345678")]
        [InlineData("12345678901234567890123456789")]
        [InlineData("123456789012345678901234567890")]
        [CLSCompliant(false)]
        public static void v_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanFormat.CheckBban(value));

        [Fact]
        public static void CheckBranchCode_ReturnsFalse_ForNull()
            => Assert.False(IbanFormat.CheckBban(null));

        [Theory]
        [MemberData(nameof(InvalidBbans), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckBban_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanFormat.CheckBban(value));

        #endregion

        #region CheckCheckDigits()

        [Fact]
        public static void CheckCheckDigit_ReturnsTrue_ForValidInput()
            => Assert.True(IbanFormat.CheckCheckDigits("12"));

        [Fact]
        public static void CheckCheckDigit_ReturnsFalse_ForNull()
            => Assert.False(IbanFormat.CheckCheckDigits(null));

        [Theory]
        [MemberData(nameof(InvalidCheckDigits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCheckDigit_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanFormat.CheckCheckDigits(value));

        #endregion

        #region CheckCountryCode()

        [Fact]
        public static void CheckCountryCode_ReturnsTrue_ForValidInput()
            => Assert.True(IbanFormat.CheckCountryCode("12"));

        [Fact]
        public static void CheckCountryCode_ReturnsFalse_ForNull()
            => Assert.False(IbanFormat.CheckCountryCode(null));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCountryCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanFormat.CheckCountryCode(value));

        #endregion

        #region CheckValue()

        [Theory]
        [MemberData(nameof(IbanFacts.ValidLengths), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanFormat.CheckValue(value));

        [Fact]
        public static void CheckValue_ReturnsFalse_ForNull()
            => Assert.False(IbanFormat.CheckValue(null));

        [Theory]
        [MemberData(nameof(IbanFacts.InvalidLengths), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanFormat.CheckValue(value));

        #endregion
    }

    public static partial class IbanFormatFacts
    {
        public static IEnumerable<object[]> InvalidBbans
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "123" };
                yield return new object[] { "1234" };
                yield return new object[] { "12345" };
                yield return new object[] { "123456" };
                yield return new object[] { "1234567" };
                yield return new object[] { "12345678" };
                yield return new object[] { "123456789" };
                yield return new object[] { "1234567890123456789012345678901" };
            }
        }

        public static IEnumerable<object[]> InvalidCheckDigits
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
            }
        }

        public static IEnumerable<object[]> InvalidCountryCodes
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
            }
        }
    }
}
