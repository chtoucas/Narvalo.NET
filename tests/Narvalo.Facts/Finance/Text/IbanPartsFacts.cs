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
        public static void CheckBranchCode_ReturnsTrue_ForValidInput(string value)
            => Assert.True(IbanParts.CheckBban(value));

        [Fact]
        public static void CheckBranchCode_ReturnsFalse_ForNull()
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
            => Assert.True(IbanParts.CheckCountryCode("12"));

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
        [InlineData("12############", "12")]
        [InlineData("12#############", "12")]
        [InlineData("12##############", "12")]
        [InlineData("12###############", "12")]
        [InlineData("12################", "12")]
        [InlineData("12#################", "12")]
        [InlineData("12##################", "12")]
        [InlineData("12###################", "12")]
        [InlineData("12####################", "12")]
        [InlineData("12#####################", "12")]
        [InlineData("12######################", "12")]
        [InlineData("12#######################", "12")]
        [InlineData("12########################", "12")]
        [InlineData("12#########################", "12")]
        [InlineData("12##########################", "12")]
        [InlineData("12###########################", "12")]
        [InlineData("12############################", "12")]
        [InlineData("12#############################", "12")]
        [InlineData("12##############################", "12")]
        [InlineData("12###############################", "12")]
        [InlineData("12################################", "12")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.ParseIntern(value, false);

            Assert.Equal(expectedValue, parts.Value.CountryCode);
        }

        [Theory]
        [InlineData("##34##########", "34")]
        [InlineData("##34###########", "34")]
        [InlineData("##34############", "34")]
        [InlineData("##34#############", "34")]
        [InlineData("##34##############", "34")]
        [InlineData("##34###############", "34")]
        [InlineData("##34################", "34")]
        [InlineData("##34#################", "34")]
        [InlineData("##34##################", "34")]
        [InlineData("##34###################", "34")]
        [InlineData("##34####################", "34")]
        [InlineData("##34#####################", "34")]
        [InlineData("##34######################", "34")]
        [InlineData("##34#######################", "34")]
        [InlineData("##34########################", "34")]
        [InlineData("##34#########################", "34")]
        [InlineData("##34##########################", "34")]
        [InlineData("##34###########################", "34")]
        [InlineData("##34############################", "34")]
        [InlineData("##34#############################", "34")]
        [InlineData("##34##############################", "34")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetCheckDigitsCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.ParseIntern(value, false);

            Assert.Equal(expectedValue, parts.Value.CheckDigits);
        }

        [Theory]
        [InlineData("####5678901234", "5678901234")]
        [InlineData("####56789012345", "56789012345")]
        [InlineData("####567890123456", "567890123456")]
        [InlineData("####5678901234567", "5678901234567")]
        [InlineData("####56789012345678", "56789012345678")]
        [InlineData("####567890123456789", "567890123456789")]
        [InlineData("####5678901234567890", "5678901234567890")]
        [InlineData("####56789012345678901", "56789012345678901")]
        [InlineData("####567890123456789012", "567890123456789012")]
        [InlineData("####5678901234567890123", "5678901234567890123")]
        [InlineData("####56789012345678901234", "56789012345678901234")]
        [InlineData("####567890123456789012345", "567890123456789012345")]
        [InlineData("####5678901234567890123456", "5678901234567890123456")]
        [InlineData("####56789012345678901234567", "56789012345678901234567")]
        [InlineData("####567890123456789012345678", "567890123456789012345678")]
        [InlineData("####5678901234567890123456789", "5678901234567890123456789")]
        [InlineData("####56789012345678901234567890", "56789012345678901234567890")]
        [InlineData("####567890123456789012345678901", "567890123456789012345678901")]
        [InlineData("####5678901234567890123456789012", "5678901234567890123456789012")]
        [InlineData("####56789012345678901234567890123", "56789012345678901234567890123")]
        [InlineData("####567890123456789012345678901234", "567890123456789012345678901234")]
        [CLSCompliant(false)]
        public static void ParseIntern_SetBbanCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.ParseIntern(value, false);

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
