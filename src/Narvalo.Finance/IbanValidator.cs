// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Applicative;
    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    public sealed class IbanValidator
    {
        private readonly bool _verifyIntegrity;
        private readonly bool _verifyISOCountryCode;
        private readonly bool _verifyBban;

        public IbanValidator(IbanValidationLevels levels)
        {
            _verifyIntegrity = levels.Contains(IbanValidationLevels.Integrity);
            _verifyISOCountryCode = levels.Contains(IbanValidationLevels.ISOCountryCode);
            _verifyBban = levels.Contains(IbanValidationLevels.Bban);
        }

        public static bool Validate(IbanParts parts, IbanValidationLevels levels)
            => new IbanValidator(levels).Validate(parts);

        public static Outcome TryValidate(IbanParts parts, IbanValidationLevels levels)
            => new IbanValidator(levels).TryValidate(parts);

        public bool Validate(IbanParts parts)
            => (!_verifyIntegrity || VerifyIntegrity(parts))
                  && (!_verifyISOCountryCode || VerifyISOCountryCode(parts))
                  && (!_verifyBban || VerifyBban(parts));

        public Outcome TryValidate(IbanParts parts)
        {
            if (_verifyIntegrity && !VerifyIntegrity(parts))
            {
                return Outcome.FromError(
                    Format.Current(Strings.IbanIntegrityCheckFailure_Format, parts.LiteralValue));
            }
            if (_verifyISOCountryCode && !VerifyISOCountryCode(parts))
            {
                return Outcome.FromError(
                    Format.Current(Strings.UnknownISOCountryCode_Format, parts.CountryCode));
            }
            if (_verifyBban && !VerifyBban(parts))
            {
                return Outcome.FromError(
                    Format.Current(Strings.BbanVerificationFailure_Format, parts.Bban));
            }

            return Outcome.Ok;
        }

        public static bool VerifyISOCountryCode(IbanParts parts)
            => CountryISOCodes.TwoLetterCodeExists(parts.CountryCode);

        public static bool VerifyBban(IbanParts parts)
        {
            throw new NotImplementedException("BBAN validation is still in the work.");
        }

        public static bool VerifyIntegrity(IbanParts parts) => IbanCheckDigits.Verify(parts.LiteralValue);
    }
}
