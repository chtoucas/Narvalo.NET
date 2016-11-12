// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Properties;

    using static System.Diagnostics.Contracts.Contract;

    public static class Int64Encoder
    {
        private const int BASE25_ALPHABET_LENGTH = 25;
        private const int BASE34_ALPHABET_LENGTH = 34;
        private const int BASE58_ALPHABET_LENGTH = 58;
        private const int FLICKR_BASE58_ALPHABET_LENGTH = 58;

        private const int BASE25_MAX_LENGTH = 14;
        private const int BASE34_MAX_LENGTH = 13;
        private const int BASE58_MAX_LENGTH = 11;
        private const int FLICKR_BASE58_MAX_LENGTH = 11;

        // On exclut la lettre "l".
        private static readonly char[] s_Base25Alphabet = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
        };

        // On exclut le chiffre zéro et la lettre "l".
        private static readonly char[] s_Base34Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
        };

        // On exclut le chiffre zéro et les lettres "I", "O" et "l".
        private static readonly char[] s_Base58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
        };

        // On exclut le chiffre zéro et les lettres "I", "O" et "l".
        private static readonly char[] s_FlickrBase58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
        };

        public static string ToBase25String(long value)
        {
            Require.Range(value >= 0L, nameof(value));
            Ensures(Result<string>() != null);

            return Encode(value, s_Base25Alphabet, BASE25_ALPHABET_LENGTH);
        }

        public static string ToBase34String(long value)
        {
            Require.Range(value >= 0L, nameof(value));
            Ensures(Result<string>() != null);

            return Encode(value, s_Base34Alphabet, BASE34_ALPHABET_LENGTH);
        }

        public static string ToBase58String(long value)
        {
            Require.Range(value >= 0L, nameof(value));
            Ensures(Result<string>() != null);

            return Encode(value, s_Base58Alphabet, BASE58_ALPHABET_LENGTH);
        }

        [SuppressMessage("Microsoft.Contracts", "Suggestion-31-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static string ToFlickrBase58String(long value)
        {
            Require.Range(value >= 0L, nameof(value));
            Ensures(Result<string>() != null);

            string retval = String.Empty;

            while (value > 0L)
            {
                long r = value % FLICKR_BASE58_ALPHABET_LENGTH;

                Assume(r < s_FlickrBase58Alphabet.Length);

                retval = s_FlickrBase58Alphabet[r].ToString() + retval;
                value /= FLICKR_BASE58_ALPHABET_LENGTH;
            }

            return retval;
        }

        public static long FromBase25String(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.True(value.Length <= BASE25_MAX_LENGTH, nameof(value));
            Ensures(Result<long>() >= 0L);

            return Decode(value, s_Base25Alphabet, BASE25_ALPHABET_LENGTH);
        }

        public static long FromBase34String(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.True(value.Length <= BASE34_MAX_LENGTH, nameof(value));
            Ensures(Result<long>() >= 0L);

            return Decode(value, s_Base34Alphabet, BASE34_ALPHABET_LENGTH);
        }

        public static long FromBase58String(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.True(value.Length <= BASE58_MAX_LENGTH, nameof(value));
            Ensures(Result<long>() >= 0L);

            return Decode(value, s_Base58Alphabet, BASE58_ALPHABET_LENGTH);
        }

        public static long FromFlickrBase58String(string value)
        {
            Require.NotNull(value, nameof(value));
            Require.True(value.Length <= FLICKR_BASE58_MAX_LENGTH, nameof(value));
            Ensures(Result<long>() >= 0L);

            long retval = 0L;
            long multiplier = 1L;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                int index = Array.IndexOf(s_FlickrBase58Alphabet, value[i]);
                if (index < 0)
                {
                    throw new FormatException(
                        Format.Resource(Strings_Core.Int64Encoder_IllegalCharacter_Format, value[i], i));
                }
                Check.True(index >= 0);

                checked
                {
                    retval += multiplier * index;
                    if (i != 0)
                    {
                        multiplier *= FLICKR_BASE58_ALPHABET_LENGTH;
                    }
                }
            }

            return retval;
        }

        internal static long Decode(string value, char[] alphabet, int alphabetLength)
        {
            Demand.NotNull(value);
            Demand.True(value.Length <= alphabetLength);
            Demand.NotNull(alphabet);
            Demand.Range(alphabetLength > 0);
            Ensures(Result<long>() >= 0L);

            long retval = 0L;
            long multiplier = 1L;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                int index = Array.BinarySearch(alphabet, value[i]);
                if (index < 0)
                {
                    throw new FormatException(
                        Format.Resource(Strings_Core.Int64Encoder_IllegalCharacter_Format, value[i], i));
                }
                Check.True(index >= 0);

                checked
                {
                    retval += multiplier * index;
                    if (i != 0)
                    {
                        multiplier *= alphabetLength;
                    }
                }
            }

            return retval;
        }

        internal static string Encode(long value, char[] alphabet, int alphabetLength)
        {
            Demand.Range(value >= 0L);
            Demand.NotNull(alphabet);
            Demand.Range(alphabetLength > 0);
            Ensures(Result<string>() != null);

            string retval = String.Empty;

            while (value > 0L)
            {
                long r = value % alphabetLength;

                Assume(r < alphabet.Length);

                retval = alphabet[r].ToString() + retval;
                value /= alphabetLength;
            }

            return retval;
        }
    }
}
