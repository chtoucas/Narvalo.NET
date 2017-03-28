// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    public static partial class BicFacts {
        [Theory]
        [BicData(nameof(BicData.ValidISOValues))]
        public static void Parse_Succeeds_ForValidISOValue(string value)
            => Assert.True(Bic.Parse(value, BicVersion.ISO).HasValue);

        [Theory]
        [BicData(nameof(BicData.ValidSwiftValues))]
        public static void Parse_Succeeds_ForValidSwiftValue(string value)
            => Assert.True(Bic.Parse(value, BicVersion.Swift).HasValue);

        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(Bic.Parse(null).HasValue);

        [Theory]
        [BicData(nameof(BicData.InvalidValues))]
        public static void Parse_ReturnsNull_ForInvalidLength(string value)
            => Assert.False(Bic.Parse(value).HasValue);

        [Theory]
        [BicData(nameof(BicData.InvalidISOValues))]
        public static void Parse_ReturnsNull_ForInvalidISOValue(string value)
            => Assert.False(Bic.Parse(value, BicVersion.ISO).HasValue);

        [Theory]
        [BicData(nameof(BicData.InvalidSwiftValues))]
        public static void Parse_ReturnsNull_ForInvalidSwiftValue(string value)
            => Assert.False(Bic.Parse(value, BicVersion.Swift).HasValue);

        [Theory]
        [InlineData("ABCDBEBB", "ABCDBEBB")]
        [InlineData("ABCDBEBBXXX", "ABCDBEBB")]
        public static void Parse_SetBusinessPartyCorrectly(string value, string expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.BusinessParty);
        }

        [Theory]
        [InlineData("ABCDBEBB", "")]
        [InlineData("ABCDBEBBXXX", "XXX")]
        public static void Parse_SetBranchCodeCorrectly(string value, string expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.BranchCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "BE")]
        [InlineData("ABCDBEBBXXX", "BE")]
        public static void Parse_SetCountryCodeCorrectly(string value, string expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.CountryCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "ABCD")]
        [InlineData("ABCDBEBBXXX", "ABCD")]
        public static void Parse_SetInstitutionCodeCorrectly(string value, string expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.InstitutionCode);
        }

        [Theory]
        [InlineData("ABCDBEBB", "BB")]
        [InlineData("ABCDBEBBXXX", "BB")]
        public static void Parse_SetLocationCodeCorrectly(string value, string expectedValue) {
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
        public static void Parse_SetIsSwiftConnectedCorrectly(string value, bool expectedValue) {
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
        public static void Parse_SetIsSwiftTestCorrectly(string value, bool expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.IsSwiftTest);
        }

        [Theory]
        [InlineData("ABCDBEBB", true)]
        [InlineData("ABCDBEBBXXX", true)]
        [InlineData("ABCDBEB1YYY", false)]
        public static void Parse_SetIsPrimaryOfficeCorrectly(string value, bool expectedValue) {
            var bic = Bic.Parse(value);

            Assert.Equal(expectedValue, bic.Value.IsPrimaryOffice);
        }

        [Theory]
        [BicData(nameof(BicData.ValidISOValues))]
        public static void Parse_ReturnsTrue_ForValidISOInput(string value) {
            var bic = Bic.Parse(value, BicVersion.ISO);

            Assert.True(bic.HasValue);
        }

        [Theory]
        [BicData(nameof(BicData.InvalidISOValues))]
        public static void Parse_ReturnsFalse_ForInvalidISOInput(string value) {
            var bic = Bic.Parse(value, BicVersion.ISO);

            Assert.False(bic.HasValue);
        }

        [Theory]
        [BicData(nameof(BicData.ValidSwiftValues))]
        public static void Parse_ReturnsTrue_ForValidSwiftInput(string value) {
            var bic = Bic.Parse(value, BicVersion.Swift);

            Assert.True(bic.HasValue);
        }

        [Theory]
        [BicData(nameof(BicData.InvalidSwiftValues))]
        public static void Parse_ReturnsFalse_ForInvalidSwiftInput(string value) {
            var bic = Bic.Parse(value, BicVersion.Swift);

            Assert.False(bic.HasValue);
        }

        [Theory]
        [BicData(nameof(BicData.ValidISOValues))]
        public static void TryParse_Succeeds_ForValidISOValue(string value)
            => Bic.TryParse(value, BicVersion.ISO);

        [Theory]
        [BicData(nameof(BicData.ValidSwiftValues))]
        public static void TryParse_Succeeds_ForValidSwiftValue(string value)
            => Bic.TryParse(value, BicVersion.Swift);

        [Fact]
        public static void TryParse_ReturnsFailure_ForNull()
            => Assert.False(Bic.TryParse(null).IsSuccess);

        [Theory]
        [BicData(nameof(BicData.InvalidValues))]
        public static void TryParse_ReturnsFailure_ForInvalidLength(string value)
            => Assert.False(Bic.TryParse(value).IsSuccess);

        [Theory]
        [BicData(nameof(BicData.InvalidISOValues))]
        public static void TryParse_ReturnsFailure_ForInvalidISOValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.ISO).IsSuccess);

        [Theory]
        [BicData(nameof(BicData.InvalidSwiftValues))]
        public static void TryParse_ReturnsFailure_ForInvalidSwiftValue(string value)
            => Assert.False(Bic.TryParse(value, BicVersion.Swift).IsSuccess);

        [Theory]
        [InlineData(null, "BE", "BB", "XXX")]
        [InlineData("ABCD", null, "BB", "XXX")]
        [InlineData("ABCD", "BE", null, "XXX")]
        [InlineData("ABCD", "BE", "BB", null)]
        public static void Create_ThrowsArgumentNullException_ForNull(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
            => Assert.Throws<ArgumentNullException>(
                () => Bic.Create(institutionCode, countryCode, locationCode, branchCode));

        [Theory]
        [BicData(nameof(BicData.InvalidInstitutionCodes))]
        public static void Create_ThrowsArgumentException_ForInvalidInstitutionCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create(value, "BE", "BB", "XXX"));

        [Theory]
        [BicData(nameof(BicData.InvalidCountryCodes))]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", value, "BB", "XXX"));

        [Theory]
        [BicData(nameof(BicData.InvalidLocationCodes))]
        public static void Create_ThrowsArgumentException_ForInvalidLocationCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", value, "XXX"));

        [Theory]
        [BicData(nameof(BicData.InvalidBranchCodes))]
        public static void Create_ThrowsArgumentException_ForInvalidBranchCodeLength(string value)
            => Assert.Throws<ArgumentException>(() => Bic.Create("ABCD", "BE", "BB", value));

        [Fact]
        public static void Create_DoesNotThrowArgumentException_ForValidInputs() {
            Bic.Create("ABCD", "BE", "BB", String.Empty);
            Bic.Create("ABCD", "BE", "BB", "XXX");
        }

        [Theory]
        [BicData(nameof(BicData.IdenticalValues))]
        public static void Equality_ReturnsTrue_ForIdenticalValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1 == bic2);
        }

        [Theory]
        [BicData(nameof(BicData.DistinctValues))]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1 == bic2);
        }

        [Theory]
        [BicData(nameof(BicData.IdenticalValues))]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1 != bic2);
        }

        [Theory]
        [BicData(nameof(BicData.DistinctValues))]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1 != bic2);
        }

        [Theory]
        [BicData(nameof(BicData.IdenticalValues))]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [BicData(nameof(BicData.DistinctValues))]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.False(bic1.Equals(bic2));
        }

        [Theory]
        [BicData(nameof(BicData.IdenticalValues))]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2) {
            var bic1 = Bic.Parse(value1).Value;
            object bic2 = Bic.Parse(value2).Value;

            Assert.True(bic1.Equals(bic2));
        }

        [Theory]
        [BicData(nameof(BicData.SampleValues))]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value) {
            var bic = Bic.Parse(value).Value;

            Assert.False(bic.Equals(null));
            Assert.False(bic.Equals(1));
            Assert.False(bic.Equals(value));
            Assert.False(bic.Equals(new Object()));
            Assert.False(bic.Equals(new My.Val(1)));
            Assert.False(bic.Equals(new My.Obj()));
        }

        [Theory]
        [BicData(nameof(BicData.SampleValues))]
        public static void Equals_IsReflexive(string value) {
            var bic = Bic.Parse(value).Value;

            Assert.True(bic.Equals(bic));
        }

        [Theory]
        [BicData(nameof(BicData.DistinctValues))]
        public static void Equals_IsAbelian(string value1, string value2) {
            var bic1a = Bic.Parse(value1).Value;
            var bic1b = Bic.Parse(value1).Value;
            var bic2 = Bic.Parse(value2).Value;

            Assert.Equal(bic1a.Equals(bic1b), bic1b.Equals(bic1a));
            Assert.Equal(bic1a.Equals(bic2), bic2.Equals(bic1a));
        }

        [Theory]
        [BicData(nameof(BicData.SampleValues))]
        public static void GetHashCode_ReturnsHashCodeValue(string value) {
            var bic = Bic.Parse(value).Value;

            Assert.Equal(value.GetHashCode(), bic.GetHashCode());
        }

        [Theory]
        [BicData(nameof(BicData.SampleValues))]
        public static void ToString_ReturnsValue(string value) {
            var bic = Bic.Parse(value).Value;

            Assert.Equal(value, bic.ToString());
        }
    }
}
