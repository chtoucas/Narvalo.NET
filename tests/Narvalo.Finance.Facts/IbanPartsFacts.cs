// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Narvalo.Finance.Properties;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static partial class IbanPartsFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(IbanParts), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(IbanParts), description) { }
        }

        [t("Try-Parse() returns null (fails) for null.")]
        public static void Parse1() {
            Assert.Null(IbanParts.Parse(null));
            Assert.False(IbanParts.TryParse(null).IsSuccess);
        }

        [T("Try-Parse() succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Parse2(string value) {
            Assert.NotNull(IbanParts.Parse(value));
            Assert.True(IbanParts.TryParse(value).IsSuccess);
        }

        [T("Try-Parse() does not perform any sort of validation, it succeeds for well-formed values having an invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumIbans))]
        public static void Parse3(string value) {
            Assert.NotNull(IbanParts.Parse(value));
            Assert.True(IbanParts.TryParse(value).IsSuccess);
        }

        [T("Parse() returns null if the input is not valid.")]
        [IbanData(nameof(IbanData.BadIbans))]
        public static void Parse4(string value) => Assert.Null(IbanParts.Parse(value));

        [T("Parse() parses the different parts correctly.")]
        [IbanData(nameof(IbanData.FakeIbans))]
        public static void Parse5(string value, string bban) {
            var parts = IbanParts.Parse(value).Value;

            Assert.Equal("FR", parts.CountryCode);
            Assert.Equal("34", parts.CheckDigits);
            Assert.Equal(bban, parts.Bban);
        }

        [T("TryParse() fails if the length of the input is not valid.")]
        [IbanData(nameof(IbanData.BadLengthIbans))]
        public static void TryParse1(string value) {
            var result = IbanParts.TryParse(value);

            Assert.True(result.IsError);

#if !NO_INTERNALS_VISIBLE_TO
            result.OnError(err => Assert.Equal(Format.Current(Strings.InvalidIbanValue, value), err));
#endif
        }

        [T("TryParse() fails if the country code is not valid.")]
        [IbanData(nameof(IbanData.BadCountryCodeIbans))]
        public static void TryParse2(string value) {
            var result = IbanParts.TryParse(value);

            Assert.True(result.IsError);

#if !NO_INTERNALS_VISIBLE_TO
            result.OnError(err => Assert.Equal(Format.Current(Strings.InvalidInput_CountryCode, value), err));
#endif
        }

        [T("TryParse() fails if the check digits are not valid.")]
        [IbanData(nameof(IbanData.BadCheckDigitsIbans))]
        public static void TryParse3(string value) {
            var result = IbanParts.TryParse(value);

            Assert.True(result.IsError);

#if !NO_INTERNALS_VISIBLE_TO
            result.OnError(err => Assert.Equal(Format.Current(Strings.InvalidInput_CheckDigits, value), err));
#endif
        }

        [T("TryParse() fails if the BBAN is not valid.")]
        [IbanData(nameof(IbanData.BadBbanIbans))]
        public static void TryParse4(string value) {
            var result = IbanParts.TryParse(value);

            Assert.True(result.IsError);

#if !NO_INTERNALS_VISIBLE_TO
            result.OnError(err => Assert.Equal(Format.Current(Strings.InvalidInput_Bban, value), err));
#endif
        }

        [T("TryParse() parses the different parts correctly.")]
        [IbanData(nameof(IbanData.FakeIbans))]
        public static void TryParse5(string value, string expectedBban) {
            var parts = IbanParts.TryParse(value);

            var countryCode = parts.Select(x => x.CountryCode);
            Assert.True(countryCode.Contains("FR"));

            var checkDigits = parts.Select(x => x.CheckDigits);
            Assert.True(checkDigits.Contains("34"));

            var bban = parts.Select(x => x.Bban);
            Assert.True(bban.Contains(expectedBban));
        }

        [t("Build() null guards")]
        public static void Build0a() {
            Assert.Throws<ArgumentNullException>("countryCode", () => IbanParts.Build(null, "20041010050500013M02606"));
            Assert.Throws<ArgumentNullException>("bban", () => IbanParts.Build("FR", null));
        }

        [T("Build() throws ArgumentException for invalid country code.")]
        [IbanData(nameof(IbanData.BadCountryCodes))]
        public static void Build0b(string value)
            => Assert.Throws<ArgumentException>("countryCode", () => IbanParts.Build(value, "20041010050500013M02606"));

        [T("Build() throws ArgumentException for invalid BBANs.")]
        [IbanData(nameof(IbanData.BadBbans))]
        public static void Build0c(string value)
            => Assert.Throws<ArgumentException>("bban", () => IbanParts.Build("FR", value));

        [T("Build() round-trip.")]
        [IbanData(nameof(IbanData.SampleIbans))]
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

        [t("Create() null guards")]
        public static void Create0a() {
            Assert.Throws<ArgumentNullException>("countryCode", () => IbanParts.Create(null, "14", "20041010050500013M02606"));
            Assert.Throws<ArgumentNullException>("checkDigits", () => IbanParts.Create("FR", null, "20041010050500013M02606"));
            Assert.Throws<ArgumentNullException>("bban", () => IbanParts.Create("FR", "14", null));
        }

        [T("Create() guards for invalid country code.")]
        [IbanData(nameof(IbanData.BadCountryCodes))]
        public static void Create0b(string value)
            => Assert.Throws<ArgumentException>("countryCode", () => IbanParts.Create(value, "14", "20041010050500013M02606"));

        [T("Create() guards for invalid check digits.")]
        [IbanData(nameof(IbanData.BadCheckDigits))]
        public static void Create0c(string value)
            => Assert.Throws<ArgumentException>("checkDigits", () => IbanParts.Create("FR", value, "20041010050500013M02606"));

        [T("Create() guards for invalid BBANs.")]
        [IbanData(nameof(IbanData.BadBbans))]
        public static void Create0d(string value)
            => Assert.Throws<ArgumentException>("bban", () => IbanParts.Create("FR", "14", value));

        [T("Create() round-trip.")]
        [IbanData(nameof(IbanData.FakeIbans))]
        public static void Create1(string value, string bban) {
            var parts = IbanParts.Parse(value).Value;
            var result = IbanParts.Create("FR", "34", bban);

            Assert.Equal(parts, result);
        }

        [T("Create() does not perform any sort of validation, it succeeds for invalid BBANs.")]
        [IbanData(nameof(IbanData.FakeBbans))]
        public static void Create2(string value)
            => Assert.DoesNotThrow(() => IbanParts.Create("FR", "14", value));

        [T("== & != for equal values.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equality1(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);

            Assert.True(parts1 == parts2);
            Assert.False(parts1 != parts2);
        }

        [T("== & !=  for distinct values.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equality2(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);

            Assert.False(parts1 == parts2);
            Assert.True(parts1 != parts2);
        }

        [T("Equals() returns true for equal values.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equals1(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);

            Assert.True(parts1.Equals(parts2));
        }

        [T("Equals() returns true for equal values after boxing.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equals2(string value1, string value2) {
            var parts1 = ParseFast(value1);
            object parts2 = ParseFast(value2);

            Assert.True(parts1.Equals(parts2));
        }

        [T("Equals() returns false for distinct values.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equals3(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);

            Assert.False(parts1.Equals(parts2));
        }

        [t("Equals(non-iban-parts) returns false.")]
        public static void Equals4() {
            var value = "AL47212110090000000235698741";
            var parts = ParseFast(value);

            Assert.False(parts.Equals(null));
            Assert.False(parts.Equals(1));
            Assert.False(parts.Equals(value));
            Assert.False(parts.Equals(new Object()));
            Assert.False(parts.Equals(new My.Val(1)));
        }

        [T("Equals() is reflexive.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Equals5(string value) {
            var parts = ParseFast(value);

            Assert.True(parts.Equals(parts));
        }

        [T("Equals() is abelian.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equals6(string value1, string value2) {
            var parts1a = ParseFast(value1);
            var parts1b = ParseFast(value1);
            var parts2 = ParseFast(value2);

            Assert.Equal(parts1a.Equals(parts1b), parts1b.Equals(parts1a));
            Assert.Equal(parts1a.Equals(parts2), parts2.Equals(parts1a));
        }

        [T("Equals() is transitive.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Equals7(string value) {
            var parts1 = ParseFast(value);
            var parts2 = ParseFast(value);
            var parts3 = ParseFast(value);

            Assert.Equal(parts1.Equals(parts2) && parts2.Equals(parts3), parts1.Equals(parts3));
        }

        [T("GetHashCode() returns the same result when called repeatedly.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void GetHashCode1(string value) {
            var parts = ParseFast(value);
            Assert.Equal(parts.GetHashCode(), parts.GetHashCode());
        }

        [T("GetHashCode() returns the same result for equal instances.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void GetHashCode2(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);
            Assert.Equal(parts1.GetHashCode(), parts2.GetHashCode());
        }

        [T("GetHashCode() returns different results for non-equal instances.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void GetHashCode3(string value1, string value2) {
            var parts1 = ParseFast(value1);
            var parts2 = ParseFast(value2);
            Assert.NotEqual(parts1.GetHashCode(), parts2.GetHashCode());
        }

        [T("ToString() guards.")]
        [InlineData(" ")]
        [InlineData("X")]
        [InlineData("XX")]
        public static void ToString0(string value)
            => Assert.Throws<FormatException>(
                () => ParseFast("AL47212110090000000235698741").ToString(value));

        [T("ToString() w/ null format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString1(string value, string formattedValue) {
            var result = ParseFast(value).ToString(null);

            Assert.Equal(formattedValue, result);
        }

        [T("ToString() w/ empty format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString2(string value, string formattedValue) {
            var result = ParseFast(value).ToString(String.Empty);

            Assert.Equal(formattedValue, result);
        }

        [T("ToString() w/ default format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString3(string value, string formattedValue) {
            var result = ParseFast(value).ToString();

            Assert.Equal(formattedValue, result);
        }

        [T("ToString() w/ general format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString4(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("G");
            var result2 = ParseFast(value).ToString("g");

            Assert.Equal(formattedValue, result1);
            Assert.Equal(formattedValue, result2);
        }

        [T("ToString() w/ human format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString5(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("H");
            var result2 = ParseFast(value).ToString("h");
            var expected = IbanParts.HumanHeader + formattedValue;

            Assert.Equal(expected, result1);
            Assert.Equal(expected, result2);
        }

        [T("ToString() w/ compact format.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString6(string value, string formattedValue) {
            var result1 = ParseFast(value).ToString("C");
            var result2 = ParseFast(value).ToString("c");

            Assert.Equal(value, result1);
            Assert.Equal(value, result2);
        }
    }

    public static partial class IbanPartsFacts {
        private static IbanParts ParseFast(string value)
            => IbanParts.Parse(value).Value;
    }
}
