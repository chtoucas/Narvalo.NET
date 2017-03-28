﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static partial class IbanFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Iban), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(Iban), description) { }
        }

        [T("CheckIntegrity() fails for invalid IBANs.")]
        [IbanData(nameof(IbanData.FakeIbans))]
        public static void CheckIntegrity1(string value, string expectedBban) {
            var iban = Iban.Parse(value, IbanStyles.None, IbanValidationLevels.None).Value;

            var result = Iban.CheckIntegrity(iban);

            Assert.False(result.HasValue);
        }

        [T("CheckIntegrity() succeeds for valid IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void CheckIntegrity2(string value) {
            var iban = Iban.Parse(value, IbanStyles.None, IbanValidationLevels.None).Value;

            var result = Iban.CheckIntegrity(iban);

            Assert.True(result.HasValue);
            Assert.True(result.Value.VerificationLevels.Contains(IbanValidationLevels.Integrity));
        }

        [T("CheckIntegrity() succeeds for already validated IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void CheckIntegrity3(string value) {
            var iban = Iban.Parse(value, IbanStyles.None, IbanValidationLevels.Integrity).Value;

            var result = Iban.CheckIntegrity(iban);

            Assert.True(result.HasValue);
            Assert.True(result.Value.VerificationLevels.Contains(IbanValidationLevels.Integrity));
        }

        [T("Build() round-trip.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Build1(string value) {
            var iban = Iban.Parse(value).Value;
            var result = Iban.Build(iban.CountryCode, iban.Bban);

            Assert.Equal(iban, result);
        }

        [T("Create() round-trip.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Create1(string value) {
            var iban = Iban.Parse(value).Value;
            var result = Iban.Create(iban.CountryCode, iban.CheckDigits, iban.Bban);

            Assert.Equal(iban, result);
        }

        [t("Try-Parse() returns null (fails) for null.")]
        public static void Parse1() {
            Assert.Null(Iban.Parse(null));
            Assert.True(Iban.TryParse(null).IsError);
        }

        [T("Try-Parse() returns null (fails) if the input is not valid.")]
        [IbanData(nameof(IbanData.BadIbans))]
        public static void Parse2a(string value) {
            Assert.Null(Iban.Parse(value));
            Assert.True(Iban.TryParse(value).IsError);
        }

        [T("Try-Parse() returns null (fails) if the input has an invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumIbans))]
        public static void Parse2b(string value) {
            Assert.Null(Iban.Parse(value, IbanValidationLevels.Integrity));
            Assert.True(Iban.TryParse(value, IbanValidationLevels.Integrity).IsError);
        }

        [T("Try-Parse() returns null (fails) if the BBAN part is invalid.")]
        [IbanData(nameof(IbanData.BadBbans))]
        public static void Parse2c(string value) {
            Assert.Null(Iban.Parse(value, IbanValidationLevels.Bban));
            Assert.True(Iban.TryParse(value, IbanValidationLevels.Bban).IsError);
        }

        [T("Tr-Parse() succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Parse3(string value) {
            Assert.NotNull(Iban.Parse(value));
            Assert.True(Iban.TryParse(value).IsSuccess);
        }

        [t("Try-Parse() returns null (fails).")]
        public static void Parse4() {
            Assert.Null(Iban.Parse(String.Empty, IbanStyles.Any));
            Assert.True(Iban.TryParse(String.Empty, IbanStyles.Any).IsError);

            Assert.Null(Iban.Parse(" ", IbanStyles.AllowWhiteSpaces));
            Assert.True(Iban.TryParse(" ", IbanStyles.AllowWhiteSpaces).IsError);

            Assert.Null(Iban.Parse("X ", IbanStyles.AllowTrailingWhite));
            Assert.True(Iban.TryParse("X ", IbanStyles.AllowLeadingWhite).IsError);

            Assert.Null(Iban.Parse(" X", IbanStyles.AllowLeadingWhite));
            Assert.True(Iban.TryParse(" X", IbanStyles.AllowLeadingWhite).IsError);

            Assert.Null(Iban.Parse(" X X ", IbanStyles.AllowLeadingWhite));
            Assert.True(Iban.TryParse(" X X ", IbanStyles.AllowLeadingWhite).IsError);

            Assert.Null(Iban.Parse("IBAN X", IbanStyles.AllowHeader));
            Assert.True(Iban.TryParse("IBAN X", IbanStyles.AllowHeader).IsError);

            Assert.True(Iban.TryParse("IBANAL47212110090000000235698741", IbanStyles.AllowHeader).IsError);
            Assert.True(Iban.TryParse("IBANAL47212110090000000235698741", IbanStyles.AllowHeader).IsError);
        }

        [T("Try-Parse(Any) succeeds.")]
        [InlineData("     AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("     AL47  2121  1009  0000  0002  3569  8741")]
        [InlineData("     AL47  2121  1009  0000  0002  3569  8741    ")]
        [InlineData("AL47 2121 1009 0000 0002 3569 8741     ")]
        [InlineData("IBAN AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("     IBAN AL472121 1009 0000 0002 3569 8741")]
        [InlineData("     IBAN AL 47 21 21 100 9 00 00  0002  3569  8741")]
        [InlineData("     IBAN AL4721 21 1009 00 00 0002 3569 8741    ")]
        public static void Parse5(string value) {
            Assert.NotNull(Iban.Parse(value, IbanStyles.Any));
            Assert.True(Iban.TryParse(value, IbanStyles.Any).IsSuccess);
        }

        [T("Try-Parse(AllowLowercaseLetter) succeeds.")]
        [InlineData("al47212110090000000235698741")]
        [InlineData("Al47212110090000000235698741")]
        [InlineData("mt84malt011000012345mtlcast001s")]
        public static void Parse6(string value) {
            Assert.NotNull(Iban.Parse(value, IbanStyles.AllowLowercaseLetter));
            Assert.True(Iban.TryParse(value, IbanStyles.AllowLowercaseLetter).IsSuccess);
        }

        [T("== & != for equal values.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equality1(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1 == iban2);
            Assert.False(iban1 != iban2);
        }

        [T("== & !=  for distinct values.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equality2(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1 == iban2);
            Assert.True(iban1 != iban2);
        }

        [T("Equals() returns true for equal values.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equals1(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [T("Equals() returns true for equal values after boxing.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void Equals2(string value1, string value2) {
            var iban1 = ParseFast(value1);
            object iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [T("Equals() returns false for distinct values.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equals3(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1.Equals(iban2));
        }

        [t("Equals(non-iban) returns false.")]
        public static void Equals4() {
            var value = "AL47212110090000000235698741";
            var iban = ParseFast(value);

            Assert.False(iban.Equals(null));
            Assert.False(iban.Equals(1));
            Assert.False(iban.Equals(value));
            Assert.False(iban.Equals(new Object()));
            Assert.False(iban.Equals(new My.Val(1)));
        }

        [T("Equals() is reflexive.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Equals5(string value) {
            var iban = ParseFast(value);

            Assert.True(iban.Equals(iban));
        }

        [T("Equals() is abelian.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void Equals6(string value1, string value2) {
            var iban1a = ParseFast(value1);
            var iban1b = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.Equal(iban1a.Equals(iban1b), iban1b.Equals(iban1a));
            Assert.Equal(iban1a.Equals(iban2), iban2.Equals(iban1a));
        }

        [T("Equals() is transitive.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Equals7(string value) {
            var iban1 = ParseFast(value);
            var iban2 = ParseFast(value);
            var iban3 = ParseFast(value);

            Assert.Equal(iban1.Equals(iban2) && iban2.Equals(iban3), iban1.Equals(iban3));
        }

        [T("GetHashCode() returns the same result when called repeatedly.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void GetHashCode1(string value) {
            var iban = ParseFast(value);
            Assert.Equal(iban.GetHashCode(), iban.GetHashCode());
        }

        [T("GetHashCode() returns the same result for equal instances.")]
        [IbanData(nameof(IbanData.FakeIdenticalIbans))]
        public static void GetHashCode2(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);
            Assert.Equal(iban1.GetHashCode(), iban2.GetHashCode());
        }

        [T("GetHashCode() returns different results for non-equal instances.")]
        [IbanData(nameof(IbanData.FakeDistinctIbans))]
        public static void GetHashCode3(string value1, string value2) {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);
            Assert.NotEqual(iban1.GetHashCode(), iban2.GetHashCode());
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

        [T("ToString() w/ null format provider.")]
        [IbanData(nameof(IbanData.FakeFormattedIbans))]
        public static void ToString7(string value, string formattedValue) {
            // NB: The format provider is actually ignored.
            var result = ParseFast(value).ToString("G", null);

            Assert.Equal(formattedValue, result);
        }
    }

    public static partial class IbanFacts {
        private static Iban ParseFast(string value)
            => Iban.Parse(value, IbanStyles.None, IbanValidationLevels.None).Value;
    }
}
