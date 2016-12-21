// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Validation
{
    using System;

    using Narvalo.Finance.Text;

    public static class IbanValidator
    {
        private const int CHECKSUM_MODULO = 97;

        public static bool Check(IbanParts parts, IbanValidations styles)
        {
            bool result = true;
            if (styles.Contains(IbanValidations.Integrity))
            {
                result = result && CheckIntegrity(parts);
            }
            if (styles.Contains(IbanValidations.ISOCountryCode))
            {
                result = result && CheckCountryCode(parts);
            }
            return result;
        }

        public static bool CheckCountryCode(IbanParts parts)
        {
            return CountryISOCodes.TwoLetterCodeExists(parts.CountryCode);
        }

        // We only verify the integrity of the whole IBAN; we do not validate the BBAN.
        // The algorithm is as follows:
        // 1. Move the leading 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resulting integer modulo 97 is equal to 1.
        // WARNING: Only works if you can ensure that "value" is only made up of alphanumeric
        // ASCII characters. Here this means that we have already called CheckValue()
        // and parts.Check().
        public static bool CheckIntegrity(IbanParts parts)
            // NB: On full .NET we have Environment.Is64BitProcess.
            // If IntPtr.Size is equal to 8, we are running in a 64-bit process and
            // we check the integrity using Int64 arithmetic; otherwize (32-bit or 16-bit process)
            // we use Int32 arithmetic (NB: IntPtr.Size = 4 in a 32-bit process). I believe,
            // but I have not verified, that ComputeInt64Checksum() is faster for a 64-bit processor.
            => CheckIntegrity(parts.LiteralValue, IntPtr.Size == 8);

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
