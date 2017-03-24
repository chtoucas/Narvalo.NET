// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class BicFacts
    {
        #region Parse()

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_ForValidISOValue(string value)
            => Assert.True(Bic.Parse(value, BicVersion.ISO).HasValue);

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_ForValidSwiftValue(string value)
            => Assert.True(Bic.Parse(value, BicVersion.Swift).HasValue);

        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(Bic.Parse(null).HasValue);

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsNull_ForInvalidLength(string value)
            => Assert.False(Bic.Parse(value).HasValue);

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsNull_ForInvalidISOValue(string value)
            => Assert.False(Bic.Parse(value, BicVersion.ISO).HasValue);

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsNull_ForInvalidSwiftValue(string value)
            => Assert.False(Bic.Parse(value, BicVersion.Swift).HasValue);

        [Theory]
        [InlineData("ABCDBEBB", "ABCDBEBB")]
        [InlineData("ABCDBEBBXXX", "ABCDBEBB")]
        [CLSCompliant(false)]
        public static void Parse_SetBusinessPartyCorrectly(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.BusinessParty);
        }

        [Theory]
        [InlineData("ABCDBEBB", "")]
        [InlineData("ABCDBEBBXXX", "XXX")]
        [CLSCompliant(false)]
        public static void Parse_SetBranchCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.BranchCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "BE")]
        [InlineData("ABCDBEBBXXX", "BE")]
        [CLSCompliant(false)]
        public static void Parse_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.CountryCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "ABCD")]
        [InlineData("ABCDBEBBXXX", "ABCD")]
        [CLSCompliant(false)]
        public static void Parse_SetInstitutionCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.InstitutionCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "BB")]
        [InlineData("ABCDBEBBXXX", "BB")]
        [CLSCompliant(false)]
        public static void Parse_SetLocationCodeCorrectly(string value, string expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.LocationCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", true)]
        [InlineData("ABCDBEBBXXX", true)]
        [InlineData("ABCDBEB0", false)]
        [InlineData("ABCDBEB0XXX", false)]
        [InlineData("ABCDBEB1", false)]
        [InlineData("ABCDBEB1XXX", false)]
        [CLSCompliant(false)]
        public static void Parse_SetIsSwiftConnectedCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.IsSwiftConnected);
        }

        [Theory]
        [InlineData("ABCDBEBB", false)]
        [InlineData("ABCDBEBBXXX", false)]
        [InlineData("ABCDBEB0", true)]
        [InlineData("ABCDBEB0XXX", true)]
        [InlineData("ABCDBEB1", false)]
        [InlineData("ABCDBEB1XXX", false)]
        [CLSCompliant(false)]
        public static void Parse_SetIsSwiftTestCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.IsSwiftTest);
        }

        [Theory]
        [InlineData("ABCDBEBB", true)]
        [InlineData("ABCDBEBBXXX", true)]
        [InlineData("ABCDBEB1YYY", false)]
        [CLSCompliant(false)]
        public static void Parse_SetIsPrimaryOfficeCorrectly(string value, bool expectedValue)
        {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.IsPrimaryOffice);
        }

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsTrue_ForValidISOInput(string value)
        {
            var bic = Bic.Parse(value, BicVersion.ISO);

            Assert.True(bic.HasValue);
        }

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsFalse_ForInvalidISOInput(string value)
        {
            var bic = Bic.Parse(value, BicVersion.ISO);

            Assert.False(bic.HasValue);
        }

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsTrue_ForValidSwiftInput(string value)
        {
            var bic = Bic.Parse(value, BicVersion.Swift);

            Assert.True(bic.HasValue);
        }

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Parse_ReturnsFalse_ForInvalidSwiftInput(string value)
        {
            var bic = Bic.Parse(value, BicVersion.Swift);

            Assert.False(bic.HasValue);
        }

        #endregion

        #region TryParse()

        [Theory]
        [MemberData(nameof(ValidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidISOValue(string value)
            => Bic.TryParse(value, BicVersion.ISO);

        [Theory]
        [MemberData(nameof(ValidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidSwiftValue(string value)
            => Bic.TryParse(value, BicVersion.Swift);

        [Fact]
        public static void TryParse_ReturnsFailure_ForNull()
            => Assert.False(Bic.TryParse(null).IsSuccess);

        [Theory]
        [MemberData(nameof(InvalidValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsFailure_ForInvalidLength(string value)
            => Assert.False(Bic.TryParse(value).IsSuccess);

        [Theory]
        [MemberData(nameof(InvalidISOValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsFailure_ForInvalidISOValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.ISO).IsSuccess);

        [Theory]
        [MemberData(nameof(InvalidSwiftValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsFailure_ForInvalidSwiftValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.Swift).IsSuccess);

        #endregion

        #region Create()

        [Theory]
        [InlineData(null, "BE", "BB", "XXX")]
        [InlineData("ABCD", null, "BB", "XXX")]
        [InlineData("ABCD", "BE", null, "XXX")]
        [InlineData("ABCD", "BE", "BB", null)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentNullException_ForNull(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
            => Assert.Throws<ArgumentNullException>(
                () => Bic.Create(institutionCode, countryCode, locationCode, branchCode));

        [Theory]
        [MemberData(nameof(InvalidInstitutionCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidInstitutionCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create(value, "BE", "BB", "XXX"));

        [Theory]
        [MemberData(nameof(InvalidCountryCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", value, "BB", "XXX"));

        [Theory]
        [MemberData(nameof(InvalidLocationCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidLocationCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", value, "XXX"));

        [Theory]
        [MemberData(nameof(InvalidBranchCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidBranchCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", "BB", value));

        [Fact]
        public static void Create_DoesNotThrowArgumentException_ForValidInputs()
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
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1 == bic2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1 == bic2);
        }

        #endregion

        #region op_Inequality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1 != bic2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1 != bic2);
        }

        #endregion

        #region Equals()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2)
        {
            var bic1 = Bic.Parse(value1).Value;
            object bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value)
        {
            var bic = Bic.Parse(value).Value;

            Assert.False(bic.Equals(null));
            Assert.False(bic.Equals(1));
            Assert.False(bic.Equals(value));
            Assert.False(bic.Equals(new Object()));
            Assert.False(bic.Equals(new My.Val(1)));
            Assert.False(bic.Equals(new My.Obj()));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsReflexive(string value)
        {
            var bic = Bic.Parse(value).Value;

            Assert.True(bic.Equals(bic));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsAbelian(string value1, string value2)
        {
            var bic1a = Bic.Parse(value1).Value;
            var bic1b = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

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
            var bic = Bic.Parse(value).Value;

            Assert.Equal(value.GetHashCode(), bic.GetHashCode());
        }

        #endregion

        #region ToString()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ReturnsValue(string value)
        {
            var bic = Bic.Parse(value).Value;

            Assert.Equal(value, bic.ToString());
        }

        #endregion
    }

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
                yield return new object[] { "123456789" };
                yield return new object[] { "1234567890" };
                yield return new object[] { "123456789012" };
            }
        }

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
