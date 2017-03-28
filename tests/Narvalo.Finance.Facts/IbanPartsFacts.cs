// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static partial class IbanPartsFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(IbanParts), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(IbanParts), description) { }
        }

        [t("Parse() returns null for null.")]
        public static void Parse1() => Assert.Null(IbanParts.Parse(null));

        [T("Parse() succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Parse2(string value) => Assert.NotNull(IbanParts.Parse(value));

        [T("Parse() does not perform any validation, it succeeds for well-formed values having an invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void Parse3(string value) => Assert.NotNull(IbanParts.Parse(value));

        [T("Parse() returns null if the length of the input is not valid.")]
        [IbanData(nameof(IbanData.BadLengthValues))]
        public static void Parse4(string value) => Assert.Null(IbanParts.Parse(value));

        [T("Parse() parses the different parts correctly.")]
        [IbanData(nameof(IbanData.FakeValues))]
        public static void Parse5(string value, string bban) {
            var parts = IbanParts.Parse(value).Value;

            Assert.Equal("FR", parts.CountryCode);
            Assert.Equal("34", parts.CheckDigits);
            Assert.Equal(bban, parts.Bban);
        }

        [t("TryParse() fails for null.")]
        public static void TryParse1() => Assert.False(IbanParts.TryParse(null).IsSuccess);

        [T("TryParse() succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void TryParse2(string value) => Assert.True(IbanParts.TryParse(value).IsSuccess);

        [T("TryParse() does not perform any validation, it succeeds for well-formed values having an invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void TryParse3(string value) => Assert.True(IbanParts.TryParse(value).IsSuccess);

        [T("TryParse() fails if the length of the input is not valid.")]
        [IbanData(nameof(IbanData.BadLengthValues))]
        public static void TryParse4(string value) => Assert.True(IbanParts.TryParse(value).IsError);

        [T("TryParse() parses the different parts correctly.")]
        [IbanData(nameof(IbanData.FakeValues))]
        public static void TryParse5(string value, string expectedValue) {
            var parts = IbanParts.TryParse(value);

            var countryCode = parts.Select(x => x.CountryCode);
            Assert.True(countryCode.Contains("FR"));

            var checkDigits = parts.Select(x => x.CheckDigits);
            Assert.True(checkDigits.Contains("34"));

            var bban = parts.Select(x => x.Bban);
            Assert.True(bban.Contains(expectedValue));
        }

        [T("Build() null guards")]
        [InlineData(null, "20041010050500013M02606")]
        [InlineData("FR", null)]
        public static void Build0a(string countryCode, string bban)
            => Assert.Throws<ArgumentNullException>(() => IbanParts.Build(countryCode, bban));

        [T("Build() throws ArgumentException for invalid country code.")]
        [IbanData(nameof(IbanData.BadCountryCodes))]
        public static void Build0b(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Build(value, "20041010050500013M02606"));

        [T("Build() throws ArgumentException for invalid BBANs.")]
        [IbanData(nameof(IbanData.BadBbans))]
        public static void Build0c(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Build("FR", value));

        [T("Build() round-trip.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Build1(string value) {
            // NB: We must use actual IBANs since Build() will compute the checksum.
            var parts = IbanParts.Parse(value).Value;
            var result = IbanParts.Build(parts.CountryCode, parts.Bban);

            Assert.Equal(parts, result);
        }

        [T("Build() does not perform any validation, it succeeds for invalid BBANs.")]
        [IbanData(nameof(IbanData.FakeBbans))]
        public static void Build2(string value)
            => Assert.DoesNotThrow(() => IbanParts.Build("FR", value));

        [T("Create() null guards")]
        [InlineData(null, "14", "20041010050500013M02606")]
        [InlineData("FR", null, "20041010050500013M02606")]
        [InlineData("FR", "14", null)]
        public static void Create0a(string countryCode, string checkDigits, string bban)
            => Assert.Throws<ArgumentNullException>(() => IbanParts.Create(countryCode, checkDigits, bban));

        [T("Create() throws ArgumentException for invalid country code.")]
        [IbanData(nameof(IbanData.BadCountryCodes))]
        public static void Create0b(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create(value, "14", "20041010050500013M02606"));

        [T("Create() throws ArgumentException for invalid check digits.")]
        [IbanData(nameof(IbanData.BadCheckDigits))]
        public static void Create0c(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", value, "20041010050500013M02606"));

        [T("Create() throws ArgumentException for invalid BBANs.")]
        [IbanData(nameof(IbanData.BadBbans))]
        public static void Create0d(string value)
            => Assert.Throws<ArgumentException>(() => IbanParts.Create("FR", "14", value));

        [T("Create() round-trip.")]
        [IbanData(nameof(IbanData.FakeValues))]
        public static void Create1(string value, string bban) {
            var parts = IbanParts.Parse(value).Value;
            var result = IbanParts.Create("FR", "34", bban);

            Assert.Equal(parts, result);
        }

        [T("Create() does not perform any validation, it succeeds for invalid BBANs.")]
        [IbanData(nameof(IbanData.FakeBbans))]
        public static void Create2(string value)
            => Assert.DoesNotThrow(() => IbanParts.Create("FR", "14", value));
    }
}
