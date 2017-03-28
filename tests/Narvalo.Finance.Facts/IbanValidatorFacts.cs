// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Narvalo.Finance.Properties;
    using Xunit;

    public static partial class IbanValidatorFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(IbanValidator), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(IbanValidator), description) { }
        }

        [T("Validate(Integrity) succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Validate1(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Integrity));

        [T("Validate(ISOCountryCode) succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void Validate2(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.ISOCountryCode));

        [t("Validate(Bban) is not implemented.")]
        public static void Validate3()
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.Validate(ParseFast("AL47212110090000000235698741"), IbanValidationLevels.Bban));

        [T("Validate(Integrity) fails for IBANs w/ invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumIbans))]
        public static void Validate4(string value)
            => Assert.False(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Integrity));

        [T("Validate(ISOCountryCode) fails for IBANs w/ invalid country code.")]
        [InlineData("ZZ345678901234")]
        public static void Validate5(string value)
            => Assert.False(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.ISOCountryCode));

        [T("TryValidate(Integrity) succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void TryValidate1(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Integrity).IsSuccess);

        [T("TryValidate(ISOCountryCode) succeeds for actual IBANs.")]
        [IbanData(nameof(IbanData.SampleIbans))]
        public static void TryValidate2(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.ISOCountryCode).IsSuccess);

        [t("TryValidate(Bban) is not implemented.")]
        public static void TryValidate3()
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.TryValidate(ParseFast("AL47212110090000000235698741"), IbanValidationLevels.Bban));

        [T("Validate(Integrity) fails for IBANs w/ invalid checksum.")]
        [IbanData(nameof(IbanData.BadChecksumIbans))]
        public static void TryValidate4(string value) {
            var result = IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Integrity);

            Assert.True(result.IsError);

            result.OnError(err => Assert.Equal(Format.Current(Strings.IbanIntegrityCheckFailure, value), err));
        }

        [T("Validate(ISOCountryCode) fails for IBANs w/ invalid country code.")]
        [InlineData("ZZ345678901234")]
        public static void TryValidate5(string value) {
            var parts = ParseFast(value);
            var result = IbanValidator.TryValidate(parts, IbanValidationLevels.ISOCountryCode);

            Assert.True(result.IsError);

            result.OnError(err => Assert.Equal(Format.Current(Strings.UnknownISOCountryCode, parts.CountryCode), err));
        }
    }

    public static partial class IbanValidatorFacts {
        private static IbanParts ParseFast(string value) => IbanParts.Parse(value).Value;
    }
}
