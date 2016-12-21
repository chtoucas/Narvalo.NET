// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Text
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class IbanPartsFacts
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
        public static void CheckBban_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanParts.CheckBban(value));

        [Fact]
        public static void CheckBban_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckBban(null));

        [Theory]
        [MemberData(nameof(InvalidBbans), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckBban_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckBban(value));

        #endregion

        #region CheckCheckDigits()

        [Fact]
        public static void CheckCheckDigit_ReturnsTrue_ForValidInput()
            => Assert.True(IbanParts.CheckCheckDigits("12"));

        [Fact]
        public static void CheckCheckDigit_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckCheckDigits(null));

        [Theory]
        [MemberData(nameof(InvalidCheckDigits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCheckDigit_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckCheckDigits(value));

        #endregion

        #region CheckCountryCode()

        [Fact]
        public static void CheckCountryCode_ReturnsTrue_ForValidInput()
            => Assert.True(IbanParts.CheckCountryCode("FR"));

        [Fact]
        public static void CheckCountryCode_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckCountryCode(null));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCountryCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckCountryCode(value));

        #endregion

        #region CheckValue()

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanParts.CheckValue(value));

        [Fact]
        public static void CheckValue_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckValue(null));

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckValue(value));

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class IbanPartsFacts
    {
        #region ParseIntern()

        [Theory]
        [InlineData("FR345678901234", "FR")]
        [InlineData("FR3456789012345", "FR")]
        [InlineData("FR34567890123456", "FR")]
        [InlineData("FR345678901234567", "FR")]
        [InlineData("FR3456789012345678", "FR")]
        [InlineData("FR34567890123456789", "FR")]
        [InlineData("FR345678901234567890", "FR")]
        [InlineData("FR3456789012345678901", "FR")]
        [InlineData("FR34567890123456789012", "FR")]
        [InlineData("FR345678901234567890123", "FR")]
        [InlineData("FR3456789012345678901234", "FR")]
        [InlineData("FR34567890123456789012345", "FR")]
        [InlineData("FR345678901234567890123456", "FR")]
        [InlineData("FR3456789012345678901234567", "FR")]
        [InlineData("FR34567890123456789012345678", "FR")]
        [InlineData("FR345678901234567890123456789", "FR")]
        [InlineData("FR3456789012345678901234567890", "FR")]
        [InlineData("FR34567890123456789012345678901", "FR")]
        [InlineData("FR345678901234567890123456789012", "FR")]
        [InlineData("FR3456789012345678901234567890123", "FR")]
        [InlineData("FR34567890123456789012345678901234", "FR")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.TryParse(value);

            Assert.Equal(expectedValue, parts.Value.CountryCode);
        }

        [Theory]
        [InlineData("FR345678901234", "34")]
        [InlineData("FR3456789012345", "34")]
        [InlineData("FR34567890123456", "34")]
        [InlineData("FR345678901234567", "34")]
        [InlineData("FR3456789012345678", "34")]
        [InlineData("FR34567890123456789", "34")]
        [InlineData("FR345678901234567890", "34")]
        [InlineData("FR3456789012345678901", "34")]
        [InlineData("FR34567890123456789012", "34")]
        [InlineData("FR345678901234567890123", "34")]
        [InlineData("FR3456789012345678901234", "34")]
        [InlineData("FR34567890123456789012345", "34")]
        [InlineData("FR345678901234567890123456", "34")]
        [InlineData("FR3456789012345678901234567", "34")]
        [InlineData("FR34567890123456789012345678", "34")]
        [InlineData("FR345678901234567890123456789", "34")]
        [InlineData("FR3456789012345678901234567890", "34")]
        [InlineData("FR34567890123456789012345678901", "34")]
        [InlineData("FR345678901234567890123456789012", "34")]
        [InlineData("FR3456789012345678901234567890123", "34")]
        [InlineData("FR34567890123456789012345678901234", "34")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetCheckDigitsCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.TryParse(value);

            Assert.Equal(expectedValue, parts.Value.CheckDigits);
        }

        [Theory]
        [InlineData("FR345678901234", "5678901234")]
        [InlineData("FR3456789012345", "56789012345")]
        [InlineData("FR34567890123456", "567890123456")]
        [InlineData("FR345678901234567", "5678901234567")]
        [InlineData("FR3456789012345678", "56789012345678")]
        [InlineData("FR34567890123456789", "567890123456789")]
        [InlineData("FR345678901234567890", "5678901234567890")]
        [InlineData("FR3456789012345678901", "56789012345678901")]
        [InlineData("FR34567890123456789012", "567890123456789012")]
        [InlineData("FR345678901234567890123", "5678901234567890123")]
        [InlineData("FR3456789012345678901234", "56789012345678901234")]
        [InlineData("FR34567890123456789012345", "567890123456789012345")]
        [InlineData("FR345678901234567890123456", "5678901234567890123456")]
        [InlineData("FR3456789012345678901234567", "56789012345678901234567")]
        [InlineData("FR34567890123456789012345678", "567890123456789012345678")]
        [InlineData("FR345678901234567890123456789", "5678901234567890123456789")]
        [InlineData("FR3456789012345678901234567890", "56789012345678901234567890")]
        [InlineData("FR34567890123456789012345678901", "567890123456789012345678901")]
        [InlineData("FR345678901234567890123456789012", "5678901234567890123456789012")]
        [InlineData("FR3456789012345678901234567890123", "56789012345678901234567890123")]
        [InlineData("FR34567890123456789012345678901234", "567890123456789012345678901234")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetBbanCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.TryParse(value);

            Assert.Equal(expectedValue, parts.Value.Bban);
        }

        #endregion
    }

#endif

    public static partial class IbanPartsFacts
    {
        public static IEnumerable<object[]> ValidValues
        {
            get
            {
                yield return new object[] { "12345678901234" };
                yield return new object[] { "123456789012345" };
                yield return new object[] { "1234567890123456" };
                yield return new object[] { "12345678901234567" };
                yield return new object[] { "123456789012345678" };
                yield return new object[] { "1234567890123456789" };
                yield return new object[] { "12345678901234567890" };
                yield return new object[] { "123456789012345678901" };
                yield return new object[] { "1234567890123456789012" };
                yield return new object[] { "12345678901234567890123" };
                yield return new object[] { "123456789012345678901234" };
                yield return new object[] { "1234567890123456789012345" };
                yield return new object[] { "12345678901234567890123456" };
                yield return new object[] { "123456789012345678901234567" };
                yield return new object[] { "1234567890123456789012345678" };
                yield return new object[] { "12345678901234567890123456789" };
                yield return new object[] { "123456789012345678901234567890" };
                yield return new object[] { "1234567890123456789012345678901" };
                yield return new object[] { "12345678901234567890123456789012" };
                yield return new object[] { "123456789012345678901234567890123" };
                yield return new object[] { "1234567890123456789012345678901234" };
            }
        }

        public static IEnumerable<object[]> InvalidValues
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
                yield return new object[] { "1234567890" };
                yield return new object[] { "12345678901" };
                yield return new object[] { "123456789012" };
                yield return new object[] { "1234567890123" };
                yield return new object[] { "12345678901234567890123456789012345" };
            }
        }

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
