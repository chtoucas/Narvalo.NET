// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Globalization;

    using Narvalo.Finance.Properties;

    // ComputeInt64Checksum() is slightly faster in a 64-bit process; it does more additions
    // but less divisions.
    // NB: On full .NET we have Environment.Is64BitProcess.
    // If IntPtr.Size is equal to 8, we are running in a 64-bit process and
    // we check the integrity using Int64 arithmetic; otherwise (32-bit or 16-bit process)
    // we use Int32 arithmetic (NB: IntPtr.Size = 4 in a 32-bit process).
    public static class IbanCheckDigits
    {
        private const int MODULUS = 97;

        public static string Compute(string countryCode, string bban)
            => Compute(countryCode, bban, IntPtr.Size == 8);

        public static string Compute(string countryCode, string bban, bool useInt64)
        {
            // No need to check bban or countryCode (except its length), we will perform
            // all necessary validations inside ComputeInt32Checksum() or ComputeInt64Checksum().
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(bban, nameof(bban));
            Require.Range(countryCode.Length == 2, nameof(countryCode));

            var iban = countryCode + "00" + bban;

            int checksum = useInt64 ? ComputeInt64Checksum(iban) : ComputeInt32Checksum(iban);
            int val = MODULUS + 1 - checksum;
            var strval = val.ToString(CultureInfo.InvariantCulture);

            return val > 9 ? strval : "0" + strval;
        }

        public static bool Verify(string iban) => Verify(iban, IntPtr.Size == 8);

        // The algorithm is as follows (ISO 7064 mod 97-10):
        // 1. Move the leading 4 chars to the end of the value.
        // 2. Replace '0' by 0, '1' by 1, etc.
        // 3. Replace 'A' by 10, 'B' by 11, etc.
        // 4. Verify that the resulting integer modulo 97 is equal to 1.
        public static bool Verify(string iban, bool useInt64)
            => (useInt64 ? ComputeInt64Checksum(iban) : ComputeInt32Checksum(iban)) == 1;

        // WARNING: Only works for well-formed values (length and valid characters).
        public static int ComputeInt32Checksum(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.Range(IbanParts.CheckLength(value), nameof(value));

            const int MAX_DIGIT = (Int32.MaxValue - 9) / 10;
            const int MAX_LETTER = (Int32.MaxValue - 35) / 100;

            int len = value.Length;
            int checksum = 0;

            for (var i = 0; i < len; i++)
            {
                char ch = i < len - 4 ? value[i + 4] : value[(i + 4) % len];
                if (ch >= '0' && ch <= '9')
                {
                    if (checksum > MAX_DIGIT) { checksum = checksum % MODULUS; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % MODULUS; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
                else
                {
                    throw new ArgumentException(
                        Format.Current(Strings.IbanValueIsNotWellFormed, value), nameof(value));
                }
            }

            return checksum % MODULUS;
        }

        // WARNING: Only works for well-formed values (length and valid characters).
        public static int ComputeInt64Checksum(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.Range(IbanParts.CheckLength(value), nameof(value));

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
                    if (checksum > MAX_DIGIT) { checksum = checksum % MODULUS; }
                    checksum = 10 * checksum + (ch - '0');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    if (checksum > MAX_LETTER) { checksum = checksum % MODULUS; }
                    checksum = 100 * checksum + (ch - 'A' + 10);
                }
                else
                {
                    throw new ArgumentException(
                        Format.Current(Strings.IbanValueIsNotWellFormed, value), nameof(value));
                }
            }

            return (int)(checksum % MODULUS);
        }
    }
}
