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

        #region Properties

        [Theory]
        [InlineData("1234#######", "1234")]
        [InlineData("1234#######", "1234")]
        [CLSCompliant(false)]
        public static void InstitutionCode(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.InstitutionCode);
            Assert.Equal(expectedValue, bic2.Value.InstitutionCode);
        }

        [Theory]
        [InlineData("####56##", "56")]
        [InlineData("####56#####", "56")]
        [CLSCompliant(false)]
        public static void CountryCode(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.CountryCode);
            Assert.Equal(expectedValue, bic2.Value.CountryCode);
        }

        [Theory]
        [InlineData("######78", "78")]
        [InlineData("######78###", "78")]
        [CLSCompliant(false)]
        public static void LocationCode(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.LocationCode);
            Assert.Equal(expectedValue, bic2.Value.LocationCode);
        }

        [Theory]
        [InlineData("########", "")]
        [InlineData("########901", "901")]
        [CLSCompliant(false)]
        public static void BranchCode(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.BranchCode);
            Assert.Equal(expectedValue, bic2.Value.BranchCode);
        }

        [Theory]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678###", "12345678")]
        [CLSCompliant(false)]
        public static void BusinessParty(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.BusinessParty);
            Assert.Equal(expectedValue, bic2.Value.BusinessParty);
        }

        [Theory]
        [InlineData("########")]
        [InlineData("###########")]
        [CLSCompliant(false)]
        public static void IsConnected(string value)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.True(bic1.IsConnected);
            Assert.True(bic2.Value.IsConnected);
        }

        [Theory]
        [InlineData("#######1")]
        [InlineData("#######1###")]
        [CLSCompliant(false)]
        public static void IsConnected_ReturnsFalse(string value)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.False(bic1.IsConnected);
            Assert.False(bic2.Value.IsConnected);
        }

        [Theory]
        [InlineData("########")]
        [InlineData("########XXX")]
        [CLSCompliant(false)]
        public static void IsPrimaryOffice(string value)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.True(bic1.IsPrimaryOffice);
            Assert.True(bic2.Value.IsPrimaryOffice);
        }

        [Theory]
        [InlineData("###########")]
        [CLSCompliant(false)]
        public static void IsPrimaryOffice_ReturnsFalse(string value)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.False(bic1.IsPrimaryOffice);
            Assert.False(bic2.Value.IsPrimaryOffice);
        }

        #endregion

        #region ToString()

        [Theory]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678901", "12345678901")]
        [CLSCompliant(false)]
        public static void ToString(string value, string expectedValue)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.Equal(expectedValue, bic1.ToString());
            Assert.Equal(expectedValue, bic2.Value.ToString());
        }

        #endregion

        #region ValidateFormat()

        [Theory]
        [InlineData("ABCDBEB1")]
        [InlineData("ABCDBEBB")]
        [InlineData("CHASUS33")]
        [InlineData("AIRFFRP1")]
        [InlineData("DEUTDEFF")]
        [InlineData("CCHBNL2A")]
        [InlineData("OKOYFIHH")]
        [InlineData("AABAFI22")]
        [InlineData("PSPBFIHH")]
        [InlineData("HANDFIHH")]
        [InlineData("DABAIE2D")]
        [InlineData("UNCRITMM")]
        [InlineData("BNORPHMM")]
        [InlineData("DEUTDEFF500")]
        [InlineData("DSBACNBXSHA")]
        [InlineData("NEDSZAJJXXX")]
        [InlineData("BSAMLKLXXXX")]
        [CLSCompliant(false)]
        public static void ValidateFormat(string value)
        {
            var bic1 = Bic.Parse(value);
            var bic2 = Bic.TryParse(value);

            Assert.True(bic1.ValidateFormat());
            Assert.True(bic2.Value.ValidateFormat());
        }

        #endregion
    }
}
