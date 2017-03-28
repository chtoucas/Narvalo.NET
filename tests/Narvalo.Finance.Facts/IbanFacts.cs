// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    public static partial class IbanFacts {
        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(Iban.Parse(null).HasValue);

        [Theory]
        [InlineData("     AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("AL47 2121 1009 0000 0002 3569 8741     ")]
        [InlineData("IBAN AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("     IBAN AL47 2121 1009 0000 0002 3569 8741")]
        public static void Parse_Succeeds_(string value)
            => Assert.True(Iban.Parse(value, IbanStyles.Any).HasValue);

        [Theory]
        [InlineData("al47212110090000000235698741")]
        [InlineData("Al47212110090000000235698741")]
        [InlineData("mt84malt011000012345mtlcast001s")]
        public static void Parse_Succeeds_Lowercase(string value)
            => Assert.True(Iban.Parse(value, IbanStyles.AllowLowercaseLetter).HasValue);

        [Fact]
        public static void TryParse_ReturnsFailure_ForNull()
            => Assert.False(Iban.TryParse(null).IsSuccess);

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void TryParse_Succeeds_ForValidInput(string value)
            => Assert.True(Iban.TryParse(value).IsSuccess);

        [Theory]
        [InlineData(null, "14", "20041010050500013M02606")]
        [InlineData("FR", null, "20041010050500013M02606")]
        [InlineData("FR", "14", null)]
        public static void Create_ThrowsArgumentNullException_ForNull(string countryCode, string checkDigits, string bban)
            => Assert.Throws<ArgumentNullException>(() => Iban.Create(countryCode, checkDigits, bban));

        [Theory]
        [IbanData(nameof(IbanData.IdenticalValues))]
        public static void Equality_ReturnsTrue_ForIdenticalValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1 == iban2);
        }

        [Theory]
        [IbanData(nameof(IbanData.DistinctValues))]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1 == iban2);
        }

        [Theory]
        [IbanData(nameof(IbanData.IdenticalValues))]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1 != iban2);
        }

        [Theory]
        [IbanData(nameof(IbanData.DistinctValues))]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1 != iban2);
        }

        [Theory]
        [IbanData(nameof(IbanData.IdenticalValues))]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [IbanData(nameof(IbanData.IdenticalValues))]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2) {
            var iban1 = ParseFast(value1);
            object iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [IbanData(nameof(IbanData.DistinctValues))]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1.Equals(iban2));
        }

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value) {
            var iban = ParseFast(value);

            Assert.False(iban.Equals(null));
            Assert.False(iban.Equals(1));
            Assert.False(iban.Equals(value));
            Assert.False(iban.Equals(new Object()));
            Assert.False(iban.Equals(new My.Val(1)));
        }

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Equals_IsReflexive(string value) {
            var iban = ParseFast(value);

            Assert.True(iban.Equals(iban));
        }

        [Theory]
        [IbanData(nameof(IbanData.DistinctValues))]
        public static void Equals_IsAbelian(string value1, string value2) {
            var iban1a = ParseFast(value1);
            var iban1b = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.Equal(iban1a.Equals(iban1b), iban1b.Equals(iban1a));
            Assert.Equal(iban1a.Equals(iban2), iban2.Equals(iban1a));
        }

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Equals_IsTransitive(string value) {
            var iban1 = ParseFast(value);
            var iban2 = ParseFast(value);
            var iban3 = ParseFast(value);

            Assert.Equal(iban1.Equals(iban2) && iban2.Equals(iban3), iban1.Equals(iban3));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("X")]
        [InlineData("XX")]
        public static void ToString_ThrowsFormatException_ForInvalidFormat(string value)
            => Assert.Throws<FormatException>(
                () => ParseFast("AL47212110090000000235698741").ToString(value));

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ForNullFormat(string value, string formattedValue) {
            var result = ParseFast(value).ToString(null);

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ForEmptyFormat(string value, string formattedValue) {
            var result = ParseFast(value).ToString(String.Empty);

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ForDefaultFormat(string value, string formattedValue) {
            var result = ParseFast(value).ToString();

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ForGeneralFormat(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("G");
            var result2 = ParseFast(value).ToString("g");

            Assert.Equal(formattedValue, result1);
            Assert.Equal(formattedValue, result2);
        }

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ForHumanFormat(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("H");
            var result2 = ParseFast(value).ToString("h");
            var expected = IbanParts.HumanHeader + formattedValue;

            Assert.Equal(expected, result1);
            Assert.Equal(expected, result2);
        }

        [Theory]
        [IbanData(nameof(IbanData.FormatSamples))]
        public static void ToString_ReturnsValue_ForCompactFormat(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("C");
            var result2 = ParseFast(value).ToString("c");

            Assert.Equal(value, result1);
            Assert.Equal(value, result2);
        }
    }

    public static partial class IbanFacts {
        private static Iban ParseFast(string value)
            => Iban.Parse(value, IbanStyles.None, IbanValidationLevels.None).Value;
    }
}
