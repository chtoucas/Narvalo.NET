// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    using Xunit;

    public static partial class BicFormatFacts
    {
        #region Validate()

        [Theory]
        [MemberData(nameof(BicFacts.ValidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, true));
        }

        [Theory]
        [MemberData(nameof(BicFacts.InvalidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsFalse_ForInvalidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(BicFormat.Validate(bic, true));
        }

        [Theory]
        [MemberData(nameof(BicFacts.ValidSwiftValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ValidateSwift_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, false));
        }

        [Theory]
        [MemberData(nameof(BicFacts.InvalidIsoValues), MemberType = typeof(BicFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ValidateSwift_ReturnsFalse_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(BicFormat.Validate(bic, false));
        }

        #endregion

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
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("1234")]
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
        [InlineData("")]
        [InlineData("1")]
        [InlineData("123")]
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
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("12345")]
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
        [InlineData("")]
        [InlineData("1")]
        [InlineData("123")]
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
}
