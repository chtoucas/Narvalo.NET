// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    public sealed class IbanValidator
    {
        private const int CHECKSUM_MODULO = 97;

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

        public static BooleanResult TryValidate(IbanParts parts, IbanValidationLevels levels)
            => new IbanValidator(levels).TryValidate(parts);

        internal static Outcome<IbanParts> TryValidateIntern(IbanParts parts, IbanValidationLevels levels)
        {
            var result = new IbanValidator(levels).TryValidate(parts);

            return result.IsTrue
                ? Outcome.Success(parts)
                : Outcome<IbanParts>.Failure(result.Message);
        }

        public bool Validate(IbanParts parts)
            => (!_verifyIntegrity || VerifyIntegrity(parts))
                  && (!_verifyISOCountryCode || VerifyISOCountryCode(parts))
                  && (!_verifyBban || VerifyBban(parts));

        public BooleanResult TryValidate(IbanParts parts)
        {
            if (_verifyIntegrity && !VerifyIntegrity(parts))
            {
                return BooleanResult.False(Strings.IbanValidator_IntegrityCheckFailure);
            }
            if (_verifyISOCountryCode && !VerifyISOCountryCode(parts))
            {
                return BooleanResult.False(Strings.IbanValidator_UnknownISOCountryCode);
            }
            if (_verifyBban && !VerifyBban(parts))
            {
                return BooleanResult.False(Strings.IbanValidator_BbanVerificationFailure);
            }

            return BooleanResult.True;
        }

        public static bool VerifyISOCountryCode(IbanParts parts)
            => CountryISOCodes.TwoLetterCodeExists(parts.CountryCode);

        public static bool VerifyBban(IbanParts parts)
        {
            throw new NotImplementedException();
        }

        // The algorithm is as follows (ISO 7064 mod 97-10):
        // 1. Move the leading 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resulting integer modulo 97 is equal to 1.
        public static bool VerifyIntegrity(IbanParts parts)
            // NB: On full .NET we have Environment.Is64BitProcess.
            // If IntPtr.Size is equal to 8, we are running in a 64-bit process and
            // we check the integrity using Int64 arithmetic; otherwise (32-bit or 16-bit process)
            // we use Int32 arithmetic (NB: IntPtr.Size = 4 in a 32-bit process). I believe,
            // but I have not verified, that ComputeInt64Checksum() is faster with a 64-bit processor.
            => CheckIntegrity(parts.LiteralValue, IntPtr.Size == 8);

        // WARNING: Only works for valid IBAN values.
        internal static bool CheckIntegrity(string value, bool sixtyfour)
        {
            Demand.NotNull(value);

            return sixtyfour
                ? ComputeInt64Checksum(value) % CHECKSUM_MODULO == 1
                : ComputeInt32Checksum(value) % CHECKSUM_MODULO == 1;
        }

        private static int ComputeInt32Checksum(string value)
        {
            Demand.NotNull(value);

            const int MAX_DIGIT = (Int32.MaxValue - 9) / 10;
            const int MAX_LETTER = (Int32.MaxValue - 35) / 100;

            int len = value.Length;
            int checksum = 0;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }

        private static long ComputeInt64Checksum(string value)
        {
            Demand.NotNull(value);

            const long MAX_DIGIT = (Int64.MaxValue - 9) / 10;
            const long MAX_LETTER = (Int64.MaxValue - 35) / 100;

            int len = value.Length;
            long checksum = 0L;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULO; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }
    }
}
