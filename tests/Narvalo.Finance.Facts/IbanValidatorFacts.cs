// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System;

    using Xunit;

    public static partial class IbanValidatorFacts {
        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Validate_ReturnsTrue_ForValidInput_Integrity(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Integrity));

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Validate_ReturnsTrue_ForValidInput_ISOCountryCode(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.ISOCountryCode));

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void Validate_ThrowsNotImplementedException_ForValidInput_Bban(string value)
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Bban));

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void TryValidate_Succeeds_ForValidInput_Integrity(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Integrity).IsSuccess);

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void TryValidate_Succeeds_ForValidInput_ISOCountryCode(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.ISOCountryCode).IsSuccess);

        [Theory]
        [IbanData(nameof(IbanData.SampleValues))]
        public static void TryValidate_ThrowsNotImplementedException_ForValidInput_Bban(string value)
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Bban));
    }

    public static partial class IbanValidatorFacts {
        private static IbanParts ParseFast(string value) => IbanParts.Parse(value).Value;
    }
}
