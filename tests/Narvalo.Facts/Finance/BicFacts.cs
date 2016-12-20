// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Validation;
    using Xunit;

    public static partial class BicFacts
    {
        #region TryParse()

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidISOValue(string value)
            => Assert.True(Bic.TryParse(value, BicVersion.ISO).HasValue);

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidSwiftValue(string value)
            => Assert.True(Bic.TryParse(value, BicVersion.Swift).HasValue);

        [Fact]
        public static void TryParse_ReturnsNull_ForNull()
            => Assert.False(Bic.TryParse(null).HasValue);

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidValues), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsNull_ForInvalidLength(string value)
            => Assert.False(Bic.TryParse(value).HasValue);

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsNull_ForInvalidISOValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.ISO).HasValue);

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsNull_ForInvalidSwiftValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.Swift).HasValue);

        #endregion

        #region Parse()

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_ForValidISOValue(string value)
            => Bic.Parse(value, BicVersion.ISO);

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_ForValidSwiftValue(string value)
            => Bic.Parse(value, BicVersion.Swift);

        [Fact]
        public static void Parse_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Bic.Parse(null));

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidValues), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ThrowsFormatException_ForInvalidLength(string value)
            => Assert.Throws<FormatException>(() => Bic.Parse(value));

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ThrowsFormatException_ForInvalidISOValue(string value)
            => Assert.Throws<FormatException>(() => Bic.Parse(value, BicVersion.ISO));

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ThrowsFormatException_ForInvalidSwiftValue(string value)
            => Assert.Throws<FormatException>(() => Bic.Parse(value, BicVersion.Swift));

        #endregion

        #region Create()

        [Fact]
        public static void Create_ThrowsArgumentNullException_ForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Bic.Create(null, "BE", "BB", "XXX"));
            Assert.Throws<ArgumentNullException>(() => Bic.Create("ABCD", null, "BB", "XXX"));
            Assert.Throws<ArgumentNullException>(() => Bic.Create("ABCD", "BE", null, "XXX"));
            Assert.Throws<ArgumentNullException>(() => Bic.Create("ABCD", "BE", "BB", null));
        }

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidInstitutionCodes), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidInstitutionCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create(value, "BE", "BB", "XXX"));

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidCountryCodes), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", value, "BB", "XXX"));

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidLocationCodes), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidLocationCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", value, "XXX"));

        [Theory]
        [MemberData(nameof(BicFormatFacts.InvalidBranchCodes), MemberType = typeof(BicFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidBranchCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", "BB", value));

        [Fact]
        public static void Create_DoesNotThrow_ForValidInputs()
        {
            Bic.Create("ABCD", "BE", "BB", String.Empty);
            Bic.Create("ABCD", "BE", "BB", "XXX");
        }

        #endregion

        #region op_Equality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.True(bic1 == bic2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.False(bic1 == bic2);
        }

        #endregion

        #region op_Inequality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.False(bic1 != bic2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.True(bic1 != bic2);
        }

        #endregion

        #region Equals()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.False(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1);
            object bic2 = Bic.Parse(value2);

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value)
        {
            var bic = Bic.Parse(value);

            Assert.False(bic.Equals(null));
            Assert.False(bic.Equals(1));
            Assert.False(bic.Equals(value));
            Assert.False(bic.Equals(new Object()));
            Assert.False(bic.Equals(new My.SimpleStruct(1)));
            Assert.False(bic.Equals(new My.SimpleValue { Value = "Whatever" }));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsReflexive(string value)
        {
            var bic = Bic.Parse(value);

            Assert.True(bic.Equals(bic));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsAbelian(string value1, string value2)
        {
            var bic1a = Bic.Parse(value1);
            var bic1b = Bic.Parse(value1);
            var bic2 = Bic.Parse(value2);

            Assert.Equal(bic1a.Equals(bic1b), bic1b.Equals(bic1a));
            Assert.Equal(bic1a.Equals(bic2), bic2.Equals(bic1a));
        }

        #endregion

        #region GetHashCode()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void GetHashCode_ReturnsHashCodeValue(string value)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(value.GetHashCode(), bic.GetHashCode());
        }

        #endregion

        #region ToString()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ReturnsValue(string value)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(value, bic.ToString());
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class BicFacts
    {
        #region ParseCore()

        [Theory]
        [InlineData("12345678", "12345678")]
        [InlineData("12345678###", "12345678")]
        [CLSCompliant(false)]
        public static void ParseCore_SetBusinessPartyCorrectly(string value, string expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.BusinessParty);
        }

        [Theory]
        [InlineData("########", "")]
        [InlineData("########901", "901")]
        [CLSCompliant(false)]
        public static void ParseCore_SetBranchCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.BranchCode);
        }

        [Theory]
        [InlineData("####56##", "56")]
        [InlineData("####56#####", "56")]
        [CLSCompliant(false)]
        public static void ParseCore_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.CountryCode);
        }

        [Theory]
        [InlineData("1234####", "1234")]
        [InlineData("1234#######", "1234")]
        [CLSCompliant(false)]
        public static void ParseCore_SetInstitutionCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.InstitutionCode);
        }

        [Theory]
        [InlineData("######78", "78")]
        [InlineData("######78###", "78")]
        [CLSCompliant(false)]
        public static void ParseCore_SetLocationCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.LocationCode);
        }

        [Theory]
        [InlineData("########", true)]
        [InlineData("###########", true)]
        [InlineData("#######0", false)]
        [InlineData("#######0###", false)]
        [InlineData("#######1", false)]
        [InlineData("#######1###", false)]
        [CLSCompliant(false)]
        public static void ParseCore_SetIsSwiftConnectedCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.IsSwiftConnected);
        }

        [Theory]
        [InlineData("########", false)]
        [InlineData("###########", false)]
        [InlineData("#######0", true)]
        [InlineData("#######0###", true)]
        [InlineData("#######1", false)]
        [InlineData("#######1###", false)]
        [CLSCompliant(false)]
        public static void ParseCore_SetIsSwiftTestCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.IsSwiftTest);
        }

        [Theory]
        [InlineData("########", true)]
        [InlineData("########XXX", true)]
        [InlineData("###########", false)]
        [CLSCompliant(false)]
        public static void ParseCore_SetIsPrimaryOfficeCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.ParseCore(value);

            Assert.Equal(expectedValue, bic.IsPrimaryOffice);
        }

        #endregion

        #region CheckParts()

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckParts_ReturnsTrue_ForValidISOInput(string value)
        {
            var bic = Bic.ParseCore(value);

            Assert.True(bic.CheckParts(BicVersion.ISO));
        }

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckParts_ReturnsFalse_ForInvalidISOInput(string value)
        {
            var bic = Bic.ParseCore(value);

            Assert.False(bic.CheckParts(BicVersion.ISO));
        }

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckParts_ReturnsTrue_ForValidSwiftInput(string value)
        {
            var bic = Bic.ParseCore(value);

            Assert.True(bic.CheckParts(BicVersion.Swift));
        }

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckParts_ReturnsFalse_ForInvalidSwiftInput(string value)
        {
            var bic = Bic.ParseCore(value);

            Assert.False(bic.CheckParts(BicVersion.Swift));
        }

        #endregion
    }

#endif

    public static partial class BicFacts
    {
        public static IEnumerable<object[]> SampleValues
        {
            get
            {
                yield return new object[] { "ABCDBEBB" };
                yield return new object[] { "ABCDBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> IdenticalValues
        {
            get
            {
                yield return new object[] { "ABCDBEBB", "ABCDBEBB" };
                yield return new object[] { "ABCDBEBBXXX", "ABCDBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> DistinctValues
        {
            get
            {
                yield return new object[] { "ABCDBEBB", "ABFRBEBB" };
                yield return new object[] { "ABCDBEBB", "ABFRBEBBXXX" };
                yield return new object[] { "ABCDBEBBXXX", "ABFRBEBB" };
                yield return new object[] { "ABCDBEBBXXX", "ABFRBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> ValidISOValues
        {
            get
            {
                // Short form.
                yield return new object[] { "ABCDBEBB" };
                // Short form: digits in prefix.
                yield return new object[] { "1BCDBEBB" };
                yield return new object[] { "11CDBEBB" };
                yield return new object[] { "111DBEBB" };
                yield return new object[] { "1111BEBB" };
                yield return new object[] { "A1CDBEBB" };
                yield return new object[] { "A11DBEBB" };
                yield return new object[] { "A111BEBB" };
                yield return new object[] { "AB1DBEBB" };
                yield return new object[] { "AB11BEBB" };
                yield return new object[] { "ABC1BEBB" };
                // Short form: digits in suffix.
                yield return new object[] { "ABCDBE1B" };
                yield return new object[] { "ABCDBE11" };
                yield return new object[] { "ABCDBEB1" };
                // Long form.
                yield return new object[] { "ABCDBEBBXXX" };
                // Long form: digit in prefix.
                yield return new object[] { "1BCDBEBBXXX" };
                yield return new object[] { "11CDBEBBXXX" };
                yield return new object[] { "111DBEBBXXX" };
                yield return new object[] { "1111BEBBXXX" };
                yield return new object[] { "A1CDBEBBXXX" };
                yield return new object[] { "A11DBEBBXXX" };
                yield return new object[] { "A111BEBBXXX" };
                yield return new object[] { "AB1DBEBBXXX" };
                yield return new object[] { "AB11BEBBXXX" };
                yield return new object[] { "ABC1BEBBXXX" };
                // Long form: digit in suffix.
                yield return new object[] { "ABCDBE1BXXX" };
                yield return new object[] { "ABCDBE11XXX" };
                yield return new object[] { "ABCDBEB1XXX" };
                // Long form: digit in branch code.
                yield return new object[] { "ABCDBEBB1XX" };
                yield return new object[] { "ABCDBEBB11X" };
                yield return new object[] { "ABCDBEBB111" };
                yield return new object[] { "ABCDBEBBX1X" };
                yield return new object[] { "ABCDBEBBX11" };
                yield return new object[] { "ABCDBEBBXX1" };
            }
        }

        public static IEnumerable<object[]> InvalidISOValues
        {
            get
            {
                // Short form: lowercase letter in prefix.
                yield return new object[] { "aBCDBEBB" };
                yield return new object[] { "AbCDBEBB" };
                yield return new object[] { "ABcDBEBB" };
                yield return new object[] { "ABCdBEBB" };
                // Short form: lowercase letter in country code.
                yield return new object[] { "ABCDbEBB" };
                yield return new object[] { "ABCDBeBB" };
                // Short form: lowercase letter in suffix.
                yield return new object[] { "ABCDBEbB" };
                yield return new object[] { "ABCDBEBb" };
                // Short form: digits in country code.
                yield return new object[] { "ABCD1EBB" };
                yield return new object[] { "ABCDB1BB" };
                // Long form: lowercase letter in branch code.
                yield return new object[] { "ABCDBEBBxXX" };
                yield return new object[] { "ABCDBEBBXxX" };
                yield return new object[] { "ABCDBEBBXXx" };
            }
        }

        public static IEnumerable<object[]> ValidSwiftValues
        {
            get
            {
                // Short form.
                yield return new object[] { "ABCDBEBB" };
                // Short form: digits in suffix.
                yield return new object[] { "ABCDBE1B" };
                yield return new object[] { "ABCDBE11" };
                yield return new object[] { "ABCDBEB1" };
                // Long form.
                yield return new object[] { "ABCDBEBBXXX" };
                // Long form: digit in suffix.
                yield return new object[] { "ABCDBE1BXXX" };
                yield return new object[] { "ABCDBE11XXX" };
                yield return new object[] { "ABCDBEB1XXX" };
                // Long form: digit in branch code.
                yield return new object[] { "ABCDBEBB1XX" };
                yield return new object[] { "ABCDBEBB11X" };
                yield return new object[] { "ABCDBEBB111" };
                yield return new object[] { "ABCDBEBBX1X" };
                yield return new object[] { "ABCDBEBBX11" };
                yield return new object[] { "ABCDBEBBXX1" };
            }
        }

        public static IEnumerable<object[]> InvalidSwiftValues
        {
            get
            {
                // Short form: digits in prefix.
                yield return new object[] { "1BCDBEBB" };
                yield return new object[] { "11CDBEBB" };
                yield return new object[] { "111DBEBB" };
                yield return new object[] { "1111BEBB" };
                yield return new object[] { "A1CDBEBB" };
                yield return new object[] { "A11DBEBB" };
                yield return new object[] { "A111BEBB" };
                yield return new object[] { "AB1DBEBB" };
                yield return new object[] { "AB11BEBB" };
                yield return new object[] { "ABC1BEBB" };
                // Long form: digit in prefix.
                yield return new object[] { "1BCDBEBBXXX" };
                yield return new object[] { "11CDBEBBXXX" };
                yield return new object[] { "111DBEBBXXX" };
                yield return new object[] { "1111BEBBXXX" };
                yield return new object[] { "A1CDBEBBXXX" };
                yield return new object[] { "A11DBEBBXXX" };
                yield return new object[] { "A111BEBBXXX" };
                yield return new object[] { "AB1DBEBBXXX" };
                yield return new object[] { "AB11BEBBXXX" };
                yield return new object[] { "ABC1BEBBXXX" };
                // Short form: lowercase letter in prefix.
                yield return new object[] { "aBCDBEBB" };
                yield return new object[] { "AbCDBEBB" };
                yield return new object[] { "ABcDBEBB" };
                yield return new object[] { "ABCdBEBB" };
                // Short form: lowercase letter in country code.
                yield return new object[] { "ABCDbEBB" };
                yield return new object[] { "ABCDBeBB" };
                // Short form: lowercase letter in suffix.
                yield return new object[] { "ABCDBEbB" };
                yield return new object[] { "ABCDBEBb" };
                // Short form: digits in country code.
                yield return new object[] { "ABCD1EBB" };
                yield return new object[] { "ABCDB1BB" };
                // Long form: lowercase letter in branch code.
                yield return new object[] { "ABCDBEBBxXX" };
                yield return new object[] { "ABCDBEBBXxX" };
                yield return new object[] { "ABCDBEBBXXx" };
            }
        }
    }
}
