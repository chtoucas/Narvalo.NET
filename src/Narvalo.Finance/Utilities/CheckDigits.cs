// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    public static class CheckDigits
    {
        private const int CHECKSUM_MODULUS = 97;

        // The algorithm is as follows (ISO 7064 mod 97-10):
        // 1. Move the leading 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resulting integer modulo 97 is equal to 1.
        public static bool CheckIntegrity(string value, bool sixtyfour)
        {
            Expect.NotNull(value);

            return sixtyfour
                ? ComputeInt64Checksum(value) % CHECKSUM_MODULUS == 1
                : ComputeInt32Checksum(value) % CHECKSUM_MODULUS == 1;
        }

        public static bool CheckIntegrity(string value)
        {
            Expect.NotNull(value);

            return ComputeInt32Checksum(value) % CHECKSUM_MODULUS == 1;
        }

        public static bool CheckIntegrity64(string value)
        {
            Expect.NotNull(value);

            return ComputeInt64Checksum(value) % CHECKSUM_MODULUS == 1;
        }

        // WARNING: Only works for well-formed values (length and valid characters).
        public static int ComputeInt32Checksum(string value)
        {
            Require.NotNull(value, nameof(value));

            const int MAX_DIGIT = (Int32.MaxValue - 9) / 10;
            const int MAX_LETTER = (Int32.MaxValue - 35) / 100;

            int len = value.Length;
            int checksum = 0;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULUS; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULUS; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }

        // WARNING: Only works for well-formed values (length and valid characters).
        public static long ComputeInt64Checksum(string value)
        {
            Require.NotNull(value, nameof(value));

            // 922 337 203 685 477 579
            const long MAX_DIGIT = (Int64.MaxValue - 9) / 10;
            // 92 233 720 368 547 757
            const long MAX_LETTER = (Int64.MaxValue - 35) / 100;

            int len = value.Length;
            long checksum = 0L;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % CHECKSUM_MODULUS; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % CHECKSUM_MODULUS; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
            }

            return checksum;
        }
    }
}
