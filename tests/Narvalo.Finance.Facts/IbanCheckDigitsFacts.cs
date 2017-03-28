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

        [t("Compute() guards.")]
        public static void Compute0() {
            Assert.Throws<ArgumentNullException>("bban", () => IbanCheckDigits.Compute("countryCode", null, true));
            Assert.Throws<ArgumentNullException>("countryCode", () => IbanCheckDigits.Compute(null, "bban", true));
            Assert.Throws<ArgumentOutOfRangeException>("countryCode", () => IbanCheckDigits.Compute("", "bban", true));
            Assert.Throws<ArgumentOutOfRangeException>("countryCode", () => IbanCheckDigits.Compute("1", "bban", true));
            Assert.Throws<ArgumentOutOfRangeException>("countryCode", () => IbanCheckDigits.Compute("123", "bban", true));
        }

        [T("Compute() succeeds.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute1(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban));
        }

        [T("Compute() succeeds using Int32 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute2(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, false));
        }

        [T("Compute() succeeds using Int64 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Compute3(string value) {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, true));
        }

        [T("Verify() returns true.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify1(string value)
            => Assert.True(IbanCheckDigits.Verify(value));

        [T("Verify() returns true using Int32 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify2(string value)
            => Assert.True(IbanCheckDigits.Verify(value, false));

        [T("Verify() returns true using Int64 arithmetic.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Verify3(string value)
            => Assert.True(IbanCheckDigits.Verify(value, true));

        [t("ComputeInt32Checksum() guards.")]
        public static void ComputeInt32Checksum0() {
            Assert.Throws<ArgumentNullException>("value", () => IbanCheckDigits.ComputeInt32Checksum(null));
            Assert.Throws<ArgumentOutOfRangeException>("value", () => IbanCheckDigits.ComputeInt32Checksum("123"));
        }

        [T("ComputeInt32Checksum() returns one.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void ComputeInt32Checksum1(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt32Checksum(value));

        [T("ComputeInt32Checksum() is not equal to one.")]
        [InlineData("123456789012345")]
        public static void ComputeInt32Checksum2(string value)
            => Assert.NotEqual(1, IbanCheckDigits.ComputeInt32Checksum(value));

        [T("ComputeInt32Checksum() throws ArgumentException when input is not well-formed.")]
        [InlineData("1234567890àçéè$")]
        public static void ComputeInt32Checksum3(string value)
            => Assert.Throws<ArgumentException>(() => IbanCheckDigits.ComputeInt32Checksum(value));

        [t("ComputeInt64Checksum() guards.")]
        public static void ComputeInt64Checksum0() {
            Assert.Throws<ArgumentNullException>("value", () => IbanCheckDigits.ComputeInt64Checksum(null));
            Assert.Throws<ArgumentOutOfRangeException>("value", () => IbanCheckDigits.ComputeInt64Checksum("123"));
        }

        [T("ComputeInt64Checksum() returns one.")]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void ComputeInt64Checksum1(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt64Checksum(value));

        [T("ComputeInt32Checksum() is not equal to one.")]
        [InlineData("123456789012345")]
        public static void ComputeInt64Checksum2(string value)
            => Assert.NotEqual(1, IbanCheckDigits.ComputeInt64Checksum(value));

        [T("ComputeInt32Checksum() throws ArgumentException when input is not well-formed.")]
        [InlineData("1234567890àçéè$")]
        public static void ComputeInt64Checksum3(string value)
            => Assert.Throws<ArgumentException>(() => IbanCheckDigits.ComputeInt64Checksum(value));
    }
}
