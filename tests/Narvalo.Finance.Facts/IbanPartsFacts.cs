// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    public static partial class IbanPartsFacts {
        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(IbanParts.Parse(null).HasValue);

        [Theory]
        [IbanData(nameof(IbanData.ValidValues))]
        public static void Parse_Succeeds_ForValidInput(string value)
            => Assert.True(IbanParts.Parse(value).HasValue);

        [Theory]
        [IbanData(nameof(IbanData.InvalidValues))]
        public static void Parse_ReturnsNull_ForInvalidInput(string value)
            => Assert.False(IbanParts.Parse(value).HasValue);

        [Theory]
        [IbanData(nameof(IbanData.ValidValues))]
        public static void Parse_SetCountryCodeCorrectly(string value) {
            var parts = IbanParts.Parse(value);

            Assert.Equal("FR", parts.Value.CountryCode);
        }

        [Theory]
        [IbanData(nameof(IbanData.ValidValues))]
        public static void Parse_SetCheckDigitsCorrectly(string value) {
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
        public static void Parse_SetBbanCorrectly(string value, string expectedValue) {
            var parts = IbanParts.Parse(value);

            Assert.Equal(expectedValue, parts.Value.Bban);
        }

        [Fact]
        public static void TryParse_ReturnsFailure_ForNull()
            => Assert.False(IbanParts.TryParse(null).IsSuccess);

        [Theory]
        [IbanData(nameof(IbanData.ValidValues))]
        public static void TryParse_ReturnsSuccess_ForValidInput(string value)
            => Assert.True(IbanParts.TryParse(value).IsSuccess);

        [Theory]
        [IbanData(nameof(IbanData.InvalidValues))]
        public static void TryParse_ReturnsFailure_ForInvalidInput(string value)
            => Assert.False(IbanParts.TryParse(value).IsSuccess);

        [Theory]
        [InlineData(null, "14", "20041010050500013M02606")]
        [InlineData("FR", null, "20041010050500013M02606")]
        [InlineData("FR", "14", null)]
        public static void Create_ThrowsArgumentNullException_ForNull(string countryCode, string checkDigits, string bban)
            => Assert.Throws<ArgumentNullException>(() => IbanParts.Create(countryCode, checkDigits, bban));

        [Theory]
        [IbanData(nameof(IbanData.InvalidCountryCodes))]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCode(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create(value, "14", "20041010050500013M02606"));

        [Theory]
        [IbanData(nameof(IbanData.InvalidCheckDigits))]
        public static void Create_ThrowsArgumentException_ForInvalidCheckDigits(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", value, "20041010050500013M02606"));

        [Theory]
        [IbanData(nameof(IbanData.InvalidBbans))]
        public static void Create_ThrowsArgumentException_ForInvalidBban(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", "14", value));

        [Fact]
        public static void Create_DoesNotThrowArgumentException_ForValidInput()
            => IbanParts.Create("FR", "14", "20041010050500013M02606");

        [Theory]
        [IbanData(nameof(IbanData.ValidBbans))]
        public static void Create_DoesNotThrowArgumentException_ForValidBban(string value)
            => IbanParts.Create("FR", "14", value);
    }
}
