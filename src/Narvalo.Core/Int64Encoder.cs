// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Int64Encoder
    {
        const int BASE25_ALPHABET_LENGTH = 25;
        const int BASE34_ALPHABET_LENGTH = 34;
        const int BASE58_ALPHABET_LENGTH = 58;
        const int FLICKR_BASE58_ALPHABET_LENGTH = 58;

        const int BASE25_MAX_LENGTH = 14;
        const int BASE34_MAX_LENGTH = 13;
        const int BASE58_MAX_LENGTH = 11;
        const int FLICKR_BASE58_MAX_LENGTH = 11;

        // On exclue la lettre "l".
        static readonly char[] s_Base25Alphabet = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et la lettre "l".
        static readonly char[] s_Base34Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] s_Base58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] s_FlickrBase58Alphabet = new char[] {
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
            Contract.Requires(value >= 0L);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, s_Base25Alphabet, BASE25_ALPHABET_LENGTH);
        }

        public static string ToBase34String(long value)
        {
            Contract.Requires(value >= 0L);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, s_Base34Alphabet, BASE34_ALPHABET_LENGTH);
        }

        public static string ToBase58String(long value)
        {
            Contract.Requires(value >= 0L);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, s_Base58Alphabet, BASE58_ALPHABET_LENGTH);
        }

        public static string ToFlickrBase58String(long value)
        {
            Require.GreaterThanOrEqualTo(value, 0L, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            string result = String.Empty;

            // TODO: Optimize.
            while (value > 0) {
                long r = value % FLICKR_BASE58_ALPHABET_LENGTH;

                Contract.Assume(r < s_FlickrBase58Alphabet.Length);

                result = s_FlickrBase58Alphabet[r].ToString() + result;
                value /= FLICKR_BASE58_ALPHABET_LENGTH;
            }

            return result;
        }

        public static long FromBase25String(string value)
        {
            Require.NotNull(value, "value");

            if (value.Length > BASE25_MAX_LENGTH) {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Int64Encoder_OutOfRangeLengthFormat, BASE25_MAX_LENGTH),
                    "value");
            }

            Contract.EndContractBlock();

            return Decode(value, s_Base25Alphabet, BASE25_ALPHABET_LENGTH);
        }

        public static long FromBase34String(string value)
        {
            Require.NotNull(value, "value");

            if (value.Length > BASE34_MAX_LENGTH) {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Int64Encoder_OutOfRangeLengthFormat, BASE34_MAX_LENGTH),
                    "value");
            }

            Contract.EndContractBlock();

            return Decode(value, s_Base34Alphabet, BASE34_ALPHABET_LENGTH);
        }

        public static long FromBase58String(string value)
        {
            Require.NotNull(value, "value");

            if (value.Length > BASE58_MAX_LENGTH) {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Int64Encoder_OutOfRangeLengthFormat, BASE58_MAX_LENGTH),
                    "value");
            }

            Contract.EndContractBlock();

            return Decode(value, s_Base58Alphabet, BASE58_ALPHABET_LENGTH);
        }

        public static long FromFlickrBase58String(string value)
        {
            Require.NotNull(value, "value");

            Contract.EndContractBlock();

            if (value.Length > FLICKR_BASE58_MAX_LENGTH) {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Int64Encoder_OutOfRangeLengthFormat, FLICKR_BASE58_MAX_LENGTH),
                    "value");
            }

            long result = 0;
            long multiplier = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.IndexOf(s_FlickrBase58Alphabet, value[i]);
                if (index == -1) {
                    throw new ArgumentException(
                        Format.CurrentCulture(Strings_Core.Int64Encoder_IllegalCharacterFormat, value[i], i),
                        "value");
                }

                checked {
                    result += multiplier * index;
                    if (i != 0) {
                        multiplier *= FLICKR_BASE58_ALPHABET_LENGTH;
                    }
                }
            }

            return result;
        }

        internal static long Decode(string value, char[] alphabet, int alphabetLength)
        {
            Require.NotNull(value, "value");
            Contract.Requires(alphabet != null);
            Contract.Requires(alphabetLength > 0);

            long result = 0;
            long multiplier = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.BinarySearch(alphabet, value[i]);
                if (index == -1) {
                    throw new ArgumentException(
                        Format.CurrentCulture(Strings_Core.Int64Encoder_IllegalCharacterFormat, value[i], i),
                        "value");
                }

                checked {
                    result += multiplier * index;
                    if (i != 0) {
                        multiplier *= alphabetLength;
                    }
                }
            }

            return result;
        }

        internal static string Encode(long value, char[] alphabet, int alphabetLength)
        {
            Require.GreaterThanOrEqualTo(value, 0L, "value");
            Contract.Requires(alphabet != null);
            Contract.Requires(alphabetLength > 0);
            Contract.Ensures(Contract.Result<string>() != null);

            string result = String.Empty;

            // TODO: Optimize.
            while (value > 0) {
                long r = value % alphabetLength;

                Contract.Assume(r < alphabet.Length);

                result = alphabet[r].ToString() + result;
                value /= alphabetLength;
            }

            return result;
        }
    }
}
