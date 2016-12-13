// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

    public static partial class BicFacts
    {
        #region TryParse()

        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678901")]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidFormat(string value)
            => Assert.True(Bic.TryParse(value).HasValue);

        [Theory]
        [InlineData(null)]
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
        public static void TryParse_ReturnsNull_ForInvalidFormat(string value)
            => Assert.False(Bic.TryParse(value).HasValue);

        #endregion

        #region Parse()

        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678901")]
        [CLSCompliant(false)]
        public static void Parse_Succeeds(string value)
            => Bic.Parse(value);

        [Fact]
        public static void Parse_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Bic.Parse(null));

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
        public static void Parse_ThrowsFormatException_ForInvalidFormat(string value)
            => Assert.Throws<FormatException>(() => Bic.Parse(value));

        #endregion

        #region Create()

        #endregion

        #region Parsing

        [Theory]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678###", "12345678")]
        [CLSCompliant(false)]
        public static void BusinessParty_AfterParsing(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.BusinessParty);
        }

        [Theory]
        [InlineData("########", "")]
        [InlineData("########901", "901")]
        [CLSCompliant(false)]
        public static void BranchCode_AfterParsing(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.BranchCode);
        }

        [Theory]
        [InlineData("####56##", "56")]
        [InlineData("####56#####", "56")]
        [CLSCompliant(false)]
        public static void CountryCode_AfterParsing(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.CountryCode);
        }

        [Theory]
        [InlineData("1234#######", "1234")]
        [InlineData("1234#######", "1234")]
        [CLSCompliant(false)]
        public static void InstitutionCode_AfterParsing(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.InstitutionCode);
        }

        [Theory]
        [InlineData("######78", "78")]
        [InlineData("######78###", "78")]
        [CLSCompliant(false)]
        public static void LocationCode_AfterParsing(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.LocationCode);
        }

        [Theory]
        [InlineData("########")]
        [InlineData("###########")]
        [CLSCompliant(false)]
        public static void IsConnected_IsTrue_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(bic.IsConnected);
        }

        [Theory]
        [InlineData("#######1")]
        [InlineData("#######1###")]
        [CLSCompliant(false)]
        public static void IsConnected_IsFalse_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(bic.IsConnected);
        }

        [Theory]
        [InlineData("########")]
        [InlineData("########XXX")]
        [CLSCompliant(false)]
        public static void IsPrimaryOffice_IsTrue_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(bic.IsPrimaryOffice);
        }

        [Theory]
        [InlineData("###########")]
        [CLSCompliant(false)]
        public static void IsPrimaryOffice_IsFalse_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(bic.IsPrimaryOffice);
        }

        #endregion

        #region ToString()

        [Theory]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678901", "12345678901")]
        [CLSCompliant(false)]
        public static void ToString(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.ToString());
        }

        #endregion

        #region ValidateFormat()

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
        public static void ValidateFormat_ReturnsTrue_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(bic.ValidateFormat());
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
        public static void ValidateFormat_ReturnsFalse_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(bic.ValidateFormat());
        }

        #endregion

        #region ValidateSwiftFormat()

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
        public static void ValidateSwiftFormat_ReturnsTrue_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(bic.ValidateSwiftFormat());
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
        public static void ValidateSwiftFormat_ReturnsFalse_AfterParsing(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(bic.ValidateSwiftFormat());
        }

        #endregion
    }
}
