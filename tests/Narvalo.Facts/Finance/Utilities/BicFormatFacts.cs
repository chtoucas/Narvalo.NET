// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class BicFormatFacts
    {
        #region CheckBranchCode()

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [CLSCompliant(false)]
        public static void CheckBranchCode_ReturnsTrue_ForValidInput(string value)
            => Assert.True(BicFormat.CheckBranchCode(value));

        [Fact]
        public static void CheckBranchCode_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckBranchCode(null));

        [Theory]
        [MemberData(nameof(InvalidBranchCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckBranchCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckBranchCode(value));

        #endregion

        #region CheckCountryCode()

        [Fact]
        public static void CheckCountryCode_ReturnsTrue_ForValidInput()
            => Assert.True(BicFormat.CheckCountryCode("12"));

        [Fact]
        public static void CheckCountryCode_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckCountryCode(null));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckCountryCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckCountryCode(value));

        #endregion

        #region CheckInstitutionCode()

        [Fact]
        public static void CheckInstitutionCode_ReturnsTrue_ForValidInput()
            => Assert.True(BicFormat.CheckInstitutionCode("1234"));

        [Fact]
        public static void CheckInstitutionCode_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckInstitutionCode(null));

        [Theory]
        [MemberData(nameof(InvalidInstitutionCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckInstitutionCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckInstitutionCode(value));

        #endregion

        #region CheckLocationCode()

        [Fact]
        public static void CheckLocationCode_ReturnsTrue_ForValidInput()
            => Assert.True(BicFormat.CheckLocationCode("12"));

        [Fact]
        public static void CheckLocationCode_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckLocationCode(null));

        [Theory]
        [MemberData(nameof(InvalidLocationCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckLocationCode_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckLocationCode(value));

        #endregion

        #region CheckValue()

        [Theory]
        [MemberData(nameof(BicFacts.ValidLengths), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsTrue_ForValidInput(string value)
            => Assert.True(BicFormat.CheckValue(value));

        [Fact]
        public static void CheckValue_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckValue(null));

        [Theory]
        [MemberData(nameof(BicFacts.InvalidLengths), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckValue(value));

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class BicFormatFacts
    {
        #region Validate()

        [Theory]
        [MemberData(nameof(BicFacts.ValidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, BicStyle.ISO));
        }

        [Theory]
        [MemberData(nameof(BicFacts.InvalidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsFalse_ForInvalidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(BicFormat.Validate(bic, BicStyle.ISO));
        }

        [Theory]
        [MemberData(nameof(BicFacts.ValidSwiftValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ValidateSwift_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, BicStyle.Swift));
        }

        [Theory]
        [MemberData(nameof(BicFacts.InvalidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ValidateSwift_ReturnsFalse_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(BicFormat.Validate(bic, BicStyle.Swift));
        }

        #endregion
    }

#endif

    public static partial class BicFormatFacts
    {
        public static IEnumerable<object[]> InvalidBranchCodes
        {
            get
            {
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "1234" };
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

        public static IEnumerable<object[]> InvalidInstitutionCodes
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "123" };
                yield return new object[] { "12345" };
            }
        }

        public static IEnumerable<object[]> InvalidLocationCodes
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