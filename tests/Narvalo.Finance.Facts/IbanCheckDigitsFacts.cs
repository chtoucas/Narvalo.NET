// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    public static partial class IbanCheckDigitsFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(IbanCheckDigits), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(IbanCheckDigits), description) { }
        }

        [t("Compute() null guards.")]
        public static void Compute0a() {
            Assert.Throws<ArgumentNullException>("bban", () => IbanCheckDigits.Compute("countryCode", null, true));
            Assert.Throws<ArgumentNullException>("countryCode", () => IbanCheckDigits.Compute(null, "bban", true));
        }

        [T("Compute() range guards.")]
        [IbanData(nameof(IbanData.BadRangeCountryCodes))]
        public static void Compute0b(string value) {
            Assert.Throws<ArgumentOutOfRangeException>("countryCode", () => IbanCheckDigits.Compute(value, "bban", true));
        }

        [T("Compute() returns the correct check digits for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute1(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban));
        }

        [T("Compute() returns the correct check digits for actual IBANs using Int32 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute2(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, false));
        }

        [T("Compute() returns the correct check digits for actual IBANs using Int64 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute3(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, true));
        }

        [T("Verify() returns true for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify1(string value)
            => Assert.True(IbanCheckDigits.Verify(value));

        [T("Verify() returns true for actual IBANs using Int32 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify2(string value)
            => Assert.True(IbanCheckDigits.Verify(value, false));

        [T("Verify() returns true for actual IBANs using Int64 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify3(string value)
            => Assert.True(IbanCheckDigits.Verify(value, true));

        [T("Verify() returns false for well-formed but invalid IBANs.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void Verify4(string value)
            => Assert.False(IbanCheckDigits.Verify(value));

        [T("Verify() returns false for well-formed but invalid IBANs using Int32 arithmetic.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void Verify5(string value)
            => Assert.False(IbanCheckDigits.Verify(value, false));

        [T("Verify() returns false for well-formed but invalid IBANs using Int64 arithmetic.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void Verify6(string value)
            => Assert.False(IbanCheckDigits.Verify(value, true));

        [t("ComputeInt32Checksum() null guards.")]
        public static void ComputeInt32Checksum0a() {
            Assert.Throws<ArgumentNullException>("value", () => IbanCheckDigits.ComputeInt32Checksum(null));
        }

        [T("ComputeInt32Checksum() range guards.")]
        [IbanData(nameof(IbanData.BadLengthValues))]
        public static void ComputeInt32Checksum0b(string value) {
            Assert.Throws<ArgumentOutOfRangeException>("value", () => IbanCheckDigits.ComputeInt32Checksum(value));
        }

        [T("ComputeInt32Checksum() returns one for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void ComputeInt32Checksum1(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt32Checksum(value));

        [T("ComputeInt32Checksum() does not return one for well-formed but invalid IBANs.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void ComputeInt32Checksum2(string value)
            => Assert.NotEqual(1, IbanCheckDigits.ComputeInt32Checksum(value));

        [T("ComputeInt32Checksum() throws ArgumentException when the value contains invalid characters.")]
        [IbanData(nameof(IbanData.BadContentValues))]
        public static void ComputeInt32Checksum3(string value)
            => Assert.Throws<ArgumentException>(() => IbanCheckDigits.ComputeInt32Checksum(value));

        [t("ComputeInt64Checksum() null guards.")]
        public static void ComputeInt64Checksum0a() {
            Assert.Throws<ArgumentNullException>("value", () => IbanCheckDigits.ComputeInt64Checksum(null));
        }

        [T("ComputeInt64Checksum() range guards.")]
        [IbanData(nameof(IbanData.BadLengthValues))]
        public static void ComputeInt64Checksum0b(string value) {
            Assert.Throws<ArgumentOutOfRangeException>("value", () => IbanCheckDigits.ComputeInt64Checksum(value));
        }

        [T("ComputeInt64Checksum() returns one.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void ComputeInt64Checksum1(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt64Checksum(value));

        [T("ComputeInt32Checksum() is not equal to one.")]
        [IbanData(nameof(IbanData.BadChecksumValues))]
        public static void ComputeInt64Checksum2(string value)
            => Assert.NotEqual(1, IbanCheckDigits.ComputeInt64Checksum(value));

        [T("ComputeInt32Checksum() throws ArgumentException when the value contains invalid characters.")]
        [IbanData(nameof(IbanData.BadContentValues))]
        public static void ComputeInt64Checksum3(string value)
            => Assert.Throws<ArgumentException>(() => IbanCheckDigits.ComputeInt64Checksum(value));
    }
}
