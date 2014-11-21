// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Int64Encoder
    {
        const int Base25AlphabetLength_ = 25;
        const int Base34AlphabetLength_ = 34;
        const int Base58AlphabetLength_ = 58;
        const int FlickrBase58AlphabetLength_ = 58;

        const int Base25MaxLength_ = 14;
        const int Base34MaxLength_ = 13;
        const int Base58MaxLength_ = 11;
        const int FlickrBase58MaxLength_ = 11;

        // On exclue la lettre "l".
        static readonly char[] Base25Alphabet_ = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et la lettre "l".
        static readonly char[] Base34Alphabet_ = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] Base58Alphabet_ = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] FlickrBase58Alphabet_ = new char[] {
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
            Contract.Requires(value >= (long)0);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, Base25Alphabet_, Base25AlphabetLength_);
        }

        public static string ToBase34String(long value)
        {
            Contract.Requires(value >= (long)0);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, Base34Alphabet_, Base34AlphabetLength_);
        }

        public static string ToBase58String(long value)
        {
            Contract.Requires(value >= (long)0);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encode(value, Base58Alphabet_, Base58AlphabetLength_);
        }

        public static string ToFlickrBase58String(long value)
        {
            Require.GreaterThanOrEqualTo(value, (long)0, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            string result = String.Empty;

            // TODO: Optimize.
            while (value > 0) {
                long r = value % FlickrBase58AlphabetLength_;

                result = FlickrBase58Alphabet_[r].ToString() + result;
                value /= FlickrBase58AlphabetLength_;
            }

            return result;
        }

        public static long FromBase25String(string value)
        {
            Require.NotNull(value, "value");
            Require.Check(
                value.Length <= Base25MaxLength_,
                "value",
                Format.CurrentCulture(SR.Int64Encoder_OutOfRangeLengthFormat, Base25MaxLength_));

            return Decode(value, Base25Alphabet_, Base25AlphabetLength_);
        }

        public static long FromBase34String(string value)
        {
            Require.NotNull(value, "value");
            Require.Check(
                value.Length <= Base34MaxLength_,
                "value",
                Format.CurrentCulture(SR.Int64Encoder_OutOfRangeLengthFormat, Base34MaxLength_));

            return Decode(value, Base34Alphabet_, Base34AlphabetLength_);
        }

        public static long FromBase58String(string value)
        {
            Require.NotNull(value, "value");
            Require.Check(
                value.Length <= Base58MaxLength_,
                "value",
                Format.CurrentCulture(SR.Int64Encoder_OutOfRangeLengthFormat, Base58MaxLength_));

            return Decode(value, Base58Alphabet_, Base58AlphabetLength_);
        }

        public static long FromFlickrBase58String(string value)
        {
            Require.NotNull(value, "value");
            Require.Check(
                value.Length <= FlickrBase58MaxLength_,
                "value",
                Format.CurrentCulture(SR.Int64Encoder_OutOfRangeLengthFormat, FlickrBase58MaxLength_));

            long result = 0;
            long multiplier = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.IndexOf(FlickrBase58Alphabet_, value[i]);
                if (index == -1) {
                    throw new ArgumentException(
                        Format.CurrentCulture(SR.Int64Encoder_IllegalCharacterFormat, value[i], i),
                        "value");
                }
                
                checked {
                    result += multiplier * index;
                    if (i != 0) {
                        multiplier *= FlickrBase58AlphabetLength_;
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
                        Format.CurrentCulture(SR.Int64Encoder_IllegalCharacterFormat, value[i], i),
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
            Require.GreaterThanOrEqualTo(value, (long)0, "value");
            Contract.Requires(alphabet != null);
            Contract.Requires(alphabetLength > 0);
            Contract.Ensures(Contract.Result<string>() != null);

            string result = String.Empty;

            // TODO: Optimize.
            while (value > 0) {
                long r = value % alphabetLength;

                result = alphabet[r].ToString() + result;
                value /= alphabetLength;
            }

            return result;
        }
    }
}
