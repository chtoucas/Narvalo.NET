// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    using Xunit;

    public static partial class BicFormatFacts
    {
        #region Validate()

        [Theory]
        [InlineData("ABCDBEBB")]
        [InlineData("ABCDBEB1")]
        [InlineData("ABCDBE11")]
        [InlineData("ABCDBE1B")]
        [InlineData("ABC1BEBB")]
        [InlineData("AB11BEBB")]
        [InlineData("A111BE1B")]
        [InlineData("1111BEBB")]
        [InlineData("ABCDBEBBXXX")]
        [InlineData("ABCDBEBBXX1")]
        [InlineData("ABCDBEBBX11")]
        [InlineData("ABCDBEBB111")]
        [InlineData("ABC1BEBBXXX")]
        [InlineData("AB11BEBBXXX")]
        [InlineData("A111BEBBXXX")]
        [InlineData("1111BEBBXXX")]
        [CLSCompliant(false)]
        public static void Validate_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, true));
        }

        [Theory]
        [InlineData("aBCDBEBB")]
        [InlineData("AbCDBEBB")]
        [InlineData("ABcDBEBB")]
        [InlineData("ABCdBEBB")]
        [InlineData("ABCDbEBB")]
        [InlineData("ABCDBeBB")]
        [InlineData("ABCDBEbB")]
        [InlineData("ABCDBEBb")]
        [InlineData("ABCD1EBB")]
        [InlineData("ABCDB1BB")]
        [InlineData("ABCDBEBBxXX")]
        [InlineData("ABCDBEBBXxX")]
        [InlineData("ABCDBEBBXXx")]
        [CLSCompliant(false)]
        public static void Validate_ReturnsFalse_ForInvalidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(BicFormat.Validate(bic, true));
        }

        [Theory]
        [InlineData("ABCDBEBB")]
        [InlineData("ABCDBEB1")]
        [InlineData("ABCDBE11")]
        [InlineData("ABCDBE1B")]
        [InlineData("ABCDBEBBXXX")]
        [InlineData("ABCDBEBBXX1")]
        [InlineData("ABCDBEBBX11")]
        [InlineData("ABCDBEBB111")]
        [CLSCompliant(false)]
        public static void ValidateSwift_ReturnsTrue_ForValidInput(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(BicFormat.Validate(bic, false));
        }

        [Theory]
        [InlineData("1BCDBEBB")]
        [InlineData("A1CDBEBB")]
        [InlineData("AB1DBEBB")]
        [InlineData("ABC1BEBB")]
        [InlineData("aBCDBEBB")]
        [InlineData("AbCDBEBB")]
        [InlineData("ABcDBEBB")]
        [InlineData("ABCdBEBB")]
        [InlineData("ABCDbEBB")]
        [InlineData("ABCDBeBB")]
        [InlineData("ABCDBEbB")]
        [InlineData("ABCDBEBb")]
        [InlineData("ABCD1EBB")]
        [InlineData("ABCDB1BB")]
        [InlineData("ABCDBEBBxXX")]
        [InlineData("ABCDBEBBXxX")]
        [InlineData("ABCDBEBBXXx")]
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
        [InlineData("12345678")]
        [InlineData("12345678901")]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsTrue_ForValidInput(string value)
            => Assert.True(BicFormat.CheckValue(value));

        [Fact]
        public static void CheckValue_ReturnsFalse_ForNull()
            => Assert.False(BicFormat.CheckValue(null));

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("1234567890")]
        [InlineData("123456789012")]
        [CLSCompliant(false)]
        public static void CheckValue_ReturnsFalse_ForInvalidInput(string value)
            => Assert.False(BicFormat.CheckValue(value));

        #endregion
    }
}
