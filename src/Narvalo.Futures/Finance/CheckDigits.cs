// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    // REVIEW: Prefer BinarySearch to IndexOf?
    public static class CheckDigits
    {
        private const int CD_LENGTH_1 = 1;
        private const int CD_LENGTH_2 = 2;

        private const int ALPHA_MODULUS = 661;          // 26 * 25 = 650
        private const int ALPHANUM_MODULUS = 1271;      // 36 * 35 = 1260
        private const int HEX_MODULUS = 251;            // 16 * 15 = 240
        private const int NUM_MODULUS = 97;             // 10 * 9 = 90

        // ABCDEFGHIJKLMNOPQRSTUVWXYZ
        private static readonly char[] s_AlphaAlphabet = new char[26] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
        };
        // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        private static readonly char[] s_AlphanumAlphabet = new char[36] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
        };
        // 0123456789ABCDEF
        private static readonly char[] s_HexAlphabet = new char[16] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F',
        };
        // 0123456789
        private static readonly char[] s_NumAlphabet = new char[10] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        };

        public static string Compute1(string value, CheckDigitsRange range)
        {
            Require.NotNull(value, nameof(value));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == CD_LENGTH_1);

            switch (range)
            {
                case CheckDigitsRange.Alphabetic:
                    Contract.Assert(s_AlphaAlphabet != null);
                    Contract.Assume(s_AlphaAlphabet.Length != 0);

                    return Compute1_(value, s_AlphaAlphabet);

                case CheckDigitsRange.Alphanumeric:
                    Contract.Assert(s_AlphanumAlphabet != null);
                    Contract.Assume(s_AlphanumAlphabet.Length != 0);

                    return Compute1_(value, s_AlphanumAlphabet);

                case CheckDigitsRange.Hexadecimal:
                    Contract.Assert(s_HexAlphabet != null);
                    Contract.Assume(s_HexAlphabet.Length != 0);

                    return Compute1_(value, s_HexAlphabet);

                case CheckDigitsRange.Numeric:
                    Contract.Assert(s_NumAlphabet != null);
                    Contract.Assume(s_NumAlphabet.Length != 0);

                    return Compute1_(value, s_NumAlphabet);

                default:
                    throw Check.Unreachable("XXX");
            }
        }

        public static string Compute2(string value, CheckDigitsRange range)
        {
            Require.NotNull(value, nameof(value));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == CD_LENGTH_2);

            switch (range)
            {
                case CheckDigitsRange.Alphabetic:
                    Contract.Assert(s_AlphaAlphabet != null);
                    Contract.Assume(s_AlphaAlphabet.Length != 0);

                    return Compute2_(value, s_AlphaAlphabet, ALPHA_MODULUS);

                case CheckDigitsRange.Alphanumeric:
                    Contract.Assert(s_AlphanumAlphabet != null);
                    Contract.Assume(s_AlphanumAlphabet.Length != 0);

                    return Compute2_(value, s_AlphanumAlphabet, ALPHANUM_MODULUS);

                case CheckDigitsRange.Hexadecimal:
                    Contract.Assert(s_HexAlphabet != null);
                    Contract.Assume(s_HexAlphabet.Length != 0);

                    return Compute2_(value, s_HexAlphabet, HEX_MODULUS);

                case CheckDigitsRange.Numeric:
                    Contract.Assert(s_NumAlphabet != null);
                    Contract.Assume(s_NumAlphabet.Length != 0);

                    return Compute2_(value, s_NumAlphabet, NUM_MODULUS);

                default:
                    throw Check.Unreachable("XXX");
            }
        }

        public static bool Verify1(string value, CheckDigitsRange range)
        {
            Require.NotNull(value, nameof(value));

            if (value.Length <= CD_LENGTH_1)
            {
                throw new ArgumentException("XXX", nameof(value));
            }

            Contract.Assert(value.Length > CD_LENGTH_1);

            string origval = value.Substring(0, value.Length - CD_LENGTH_1);

            return value.Substring(value.Length - CD_LENGTH_1) == Compute1(origval, range);
        }

        public static bool Verify2(string value, CheckDigitsRange range)
        {
            Require.NotNull(value, nameof(value));

            if (value.Length <= CD_LENGTH_2)
            {
                throw new ArgumentException("XXX", nameof(value));
            }

            Contract.Assert(value.Length > CD_LENGTH_2);

            string origval = value.Substring(0, value.Length - CD_LENGTH_2);

            return value.Substring(value.Length - CD_LENGTH_2) == Compute2(origval, range);
        }

        private static string Compute1_(string value, char[] alphabet)
        {
            Contract.Requires(value != null);
            Contract.Requires(alphabet != null);
            Contract.Requires(alphabet.Length != 0);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == CD_LENGTH_1);

            int alength = alphabet.Length;
            int pos = alength;

            for (int i = 0; i < value.Length; i++)
            {
                int index = Array.BinarySearch(alphabet, value[i]);
                if (index < 0)
                {
                    throw new ArgumentException("XXX", nameof(value));
                }

                //Contract.Assert(index >= 0);
                //Contract.Assert(index < alength);

                pos += index;

                if (pos > alength)
                {
                    pos -= alength;
                }

                //Contract.Assert(pos <= alength);

                pos *= 2;

                if (pos > alength)
                {
                    pos -= alength + 1;
                }

                //Contract.Assert(pos <= alength);
            }

            pos = (alength - pos + 1) % alength;

            Contract.Assert(pos >= 0);
            Contract.Assert(pos < alength);

            return new String(new char[CD_LENGTH_1] { alphabet[pos] });
        }

        private static string Compute2_(string value, char[] alphabet, int modulus)
        {
            Contract.Requires(value != null);
            Contract.Requires(alphabet != null);
            Contract.Requires(alphabet.Length != 0);
            Contract.Requires(modulus > 0);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == CD_LENGTH_2);

            int alength = alphabet.Length;
            int pos = 0;

            for (int i = 0; i < value.Length; i++)
            {
                int index = Array.BinarySearch(alphabet, value[i]);
                if (index < 0)
                {
                    throw new ArgumentException("XXX", nameof(value));
                }

                //Contract.Assert(index >= 0);
                //Contract.Assert(index < alength);

                pos = ((pos + index) * alength) % modulus;
            }

            pos = (pos * alength) % modulus;
            pos = (1 + modulus - pos) % modulus;

            int r = pos % alength;
            int q = (pos - r) / alength;

            Contract.Assume(q >= 0);
            Contract.Assume(q < alength);
            Contract.Assert(r >= 0);
            Contract.Assert(r < alength);

            return new String(new char[CD_LENGTH_2] { alphabet[q], alphabet[r] });
        }
    }
}
