// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class IbanPartsFacts
    {
        #region CheckBban()

        [Fact]
        public static void CheckBban_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckBban(null));

        [Theory]
        [MemberData(nameof(ValidBbans), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckBban_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanParts.CheckBban(value));

        [Theory]
        [MemberData(nameof(InvalidBbans), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckBban_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckBban(value));

        #endregion

        #region CheckCheckDigits()

        [Fact]
        public static void CheckCheckDigit_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckCheckDigits(null));

        [Fact]
        public static void CheckCheckDigit_ReturnsTrue_ForValidInput()
            => Assert.True(IbanParts.CheckCheckDigits("12"));

        [Theory]
        [MemberData(nameof(InvalidCheckDigits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCheckDigit_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckCheckDigits(value));

        #endregion

        #region CheckCountryCode()

        [Fact]
        public static void CheckCountryCode_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckCountryCode(null));

        [Fact]
        public static void CheckCountryCode_ReturnsTrue_ForValidInput()
            => Assert.True(IbanParts.CheckCountryCode("FR"));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCountryCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckCountryCode(value));

        #endregion

        #region CheckValue()

        [Fact]
        public static void CheckValue_ReturnsFalse_ForNull()
            => Assert.False(IbanParts.CheckValue(null));

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanParts.CheckValue(value));

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(IbanParts.CheckValue(value));

        #endregion

        #region Parse()

        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(IbanParts.Parse(null).HasValue);

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_ForValidInput(string value)
            => Assert.True(IbanParts.Parse(value).HasValue);

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsNull_ForInvalidInput(string value)
            => Assert.False(IbanParts.Parse(value).HasValue);

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_SetCountryCodeCorrectly(string value)
        {
            var parts = IbanParts.Parse(value);

            Assert.Equal("FR", parts.Value.CountryCode);
        }

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_SetCheckDigitsCorrectly(string value)
        {
            var parts = IbanParts.Parse(value);

            Assert.Equal("34", parts.Value.CheckDigits);
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
        public static void Parse_SetBbanCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.Parse(value);

            Assert.Equal(expectedValue, parts.Value.Bban);
        }

        #endregion

        #region TryParse()

        [Fact]
        public static void TryParse_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => IbanParts.TryParse(null));

        [Theory]
        [MemberData(nameof(ValidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsSuccess_ForValidInput(string value)
            => Assert.True(IbanParts.TryParse(value).Success);

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsFailure_ForInvalidInput(string value)
            => Assert.False(IbanParts.TryParse(value).Success);

        #endregion

        #region Create()

        [Theory]
        [InlineData(null, "14", "20041010050500013M02606")]
        [InlineData("FR", null, "20041010050500013M02606")]
        [InlineData("FR", "14", null)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentNullException_ForNull(string countryCode, string checkDigits, string bban)
            => Assert.Throws<ArgumentNullException>(() => IbanParts.Create(countryCode, checkDigits, bban));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCode(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create(value, "14", "20041010050500013M02606"));

        [Theory]
        [MemberData(nameof(InvalidCheckDigits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCheckDigits(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", value, "20041010050500013M02606"));

        [Theory]
        [MemberData(nameof(InvalidBbans), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidBban(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", "14", value));

        [Fact]
        public static void Create_DoesNotThrowArgumentException_ForValidInput()
            => IbanParts.Create("FR", "14", "20041010050500013M02606");

        #endregion
    }

    public static partial class IbanPartsFacts
    {
        public static IEnumerable<object[]> ValidValues
        {
            get
            {
                yield return new object[] { "FR345678901234" };
                yield return new object[] { "FR3456789012345" };
                yield return new object[] { "FR34567890123456" };
                yield return new object[] { "FR345678901234567" };
                yield return new object[] { "FR3456789012345678" };
                yield return new object[] { "FR34567890123456789" };
                yield return new object[] { "FR345678901234567890" };
                yield return new object[] { "FR3456789012345678901" };
                yield return new object[] { "FR34567890123456789012" };
                yield return new object[] { "FR345678901234567890123" };
                yield return new object[] { "FR3456789012345678901234" };
                yield return new object[] { "FR34567890123456789012345" };
                yield return new object[] { "FR345678901234567890123456" };
                yield return new object[] { "FR3456789012345678901234567" };
                yield return new object[] { "FR34567890123456789012345678" };
                yield return new object[] { "FR345678901234567890123456789" };
                yield return new object[] { "FR3456789012345678901234567890" };
                yield return new object[] { "FR34567890123456789012345678901" };
                yield return new object[] { "FR345678901234567890123456789012" };
                yield return new object[] { "FR3456789012345678901234567890123" };
                yield return new object[] { "FR34567890123456789012345678901234" };
            }
        }

        public static IEnumerable<object[]> InvalidValues
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "F" };
                yield return new object[] { "FR" };
                yield return new object[] { "FR3" };
                yield return new object[] { "FR34" };
                yield return new object[] { "FR345" };
                yield return new object[] { "FR3456" };
                yield return new object[] { "FR34567" };
                yield return new object[] { "FR345678" };
                yield return new object[] { "FR3456789" };
                yield return new object[] { "FR34567890" };
                yield return new object[] { "FR345678901" };
                yield return new object[] { "FR3456789012" };
                yield return new object[] { "FR34567890123" };
                yield return new object[] { "FR345678901234567890123456789012345" };
            }
        }

        public static IEnumerable<object[]> ValidBbans
        {
            get
            {
                yield return new object[] { "1234567890" };
                yield return new object[] { "12345678901" };
                yield return new object[] { "123456789012" };
                yield return new object[] { "1234567890123" };
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
            }
        }

        public static IEnumerable<object[]> InvalidBbans
        {
            get
            {
                // Lengths.
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
                // Content.
                yield return new object[] { "         " };
                yield return new object[] { "---------" };
            }
        }

        public static IEnumerable<object[]> InvalidCheckDigits
        {
            get
            {
                // Lengths.
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
                // Content.
                yield return new object[] { "  " };
                yield return new object[] { "--" };
                yield return new object[] { "AB" };
            }
        }

        public static IEnumerable<object[]> InvalidCountryCodes
        {
            get
            {
                // Lengths.
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
                // Content.
                yield return new object[] { "  " };
                yield return new object[] { "--" };
            }
        }
    }
}
