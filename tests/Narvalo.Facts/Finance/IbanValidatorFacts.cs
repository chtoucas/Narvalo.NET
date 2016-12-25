// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

    public static partial class IbanValidatorFacts
    {
        #region Validate()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsTrue_ForValidInput_Integrity(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Integrity));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ReturnsTrue_ForValidInput_ISOCountryCode(string value)
            => Assert.True(IbanValidator.Validate(ParseFast(value), IbanValidationLevels.ISOCountryCode));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Validate_ThrowsNotImplementedException_ForValidInput_Bban(string value)
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.Validate(ParseFast(value), IbanValidationLevels.Bban));

        #endregion

        #region TryValidate()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidate_Succeeds_ForValidInput_Integrity(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Integrity).IsTrue);

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidate_Succeeds_ForValidInput_ISOCountryCode(string value)
            => Assert.True(IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.ISOCountryCode).IsTrue);

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidate_ThrowsNotImplementedException_ForValidInput_Bban(string value)
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.TryValidate(ParseFast(value), IbanValidationLevels.Bban));

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class IbanValidatorFacts
    {
        #region TryValidateIntern()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidateIntern_Succeeds_ForValidInput_Integrity(string value)
            => Assert.True(IbanValidator.TryValidateIntern(ParseFast(value), IbanValidationLevels.Integrity).Success);

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidateIntern_Succeeds_ForValidInput_ISOCountryCode(string value)
            => Assert.True(IbanValidator.TryValidateIntern(ParseFast(value), IbanValidationLevels.ISOCountryCode).Success);

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryValidateIntern_ThrowsNotImplementedException_ForValidInput_Bban(string value)
            => Assert.Throws<NotImplementedException>(
                () => IbanValidator.TryValidateIntern(ParseFast(value), IbanValidationLevels.Bban));

        #endregion

        #region CheckIntegrity()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt32Arithmetic(string value)
            => Assert.True(IbanValidator.CheckIntegrity(value, false));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt64Arithmetic(string value)
            => Assert.True(IbanValidator.CheckIntegrity(value, true));

        #endregion
    }

#endif

    public static partial class IbanValidatorFacts
    {
        private static IbanParts ParseFast(string value)
            => IbanParts.Parse(value).Value;
    }
}
