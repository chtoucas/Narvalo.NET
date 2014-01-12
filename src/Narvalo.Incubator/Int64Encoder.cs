namespace Narvalo
{
    using System;
    using Narvalo.Diagnostics;

    public static class Int64Encoder
    {
        const int Base25AlphabetLength = 25;
        const int Base34AlphabetLength = 34;
        const int Base58AlphabetLength = 58;
        const int FlickrBase58AlphabetLength = 58;

        const int Base25MaxLength = 14;
        const int Base34MaxLength = 13;
        const int Base58MaxLength = 11;
        const int FlickrBase58MaxLength = 11;

        // On exclue la lettre "l".
        static readonly char[] Base25Alphabet = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et la lettre "l".
        static readonly char[] Base34Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] Base58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 
            'v', 'w', 'x', 'y', 'z', 
        };

        // On exclue le chiffre zéro et les lettres "I", "O" et "l".
        static readonly char[] FlickrBase58Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
            'W', 'X', 'Y', 'Z',
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string ToBase25String(long value)
        {
            Requires.GreaterThanOrEqualTo(value, 0, "value");

            return Encode(value, Base25Alphabet, Base25AlphabetLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string ToBase34String(long value)
        {
            Requires.GreaterThanOrEqualTo(value, 0, "value");

            return Encode(value, Base34Alphabet, Base34AlphabetLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string ToBase58String(long value)
        {
            Requires.GreaterThanOrEqualTo(value, 0, "value");

            return Encode(value, Base58Alphabet, Base58AlphabetLength);
        }

        public static string ToFlickrBase58String(long value)
        {
            Requires.GreaterThanOrEqualTo(value, 0, "value");

            string result = String.Empty;

            while (value > 0) {
                long r = value % FlickrBase58AlphabetLength;
                result = FlickrBase58Alphabet[r] + result;
                value /= FlickrBase58AlphabetLength;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public static long FromBase25String(string value)
        {
            Requires.NotNull(value, "value");
            Requires.LessThanOrEqualTo(value.Length, Base25MaxLength, "value");

            return Decode(value, Base25Alphabet, Base25AlphabetLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public static long FromBase34String(string value)
        {
            Requires.NotNull(value, "value");
            Requires.LessThanOrEqualTo(value.Length, Base34MaxLength, "value");

            return Decode(value, Base34Alphabet, Base34AlphabetLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public static long FromBase58String(string value)
        {
            Requires.NotNull(value, "value");
            Requires.LessThanOrEqualTo(value.Length, Base58MaxLength, "value");

            return Decode(value, Base58Alphabet, Base58AlphabetLength);
        }

        public static long FromFlickrBase58String(string value)
        {
            Requires.NotNull(value, "value");
            Requires.LessThanOrEqualTo(value.Length, FlickrBase58MaxLength, "value");

            long result = 0;
            long multiplier  = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.IndexOf(FlickrBase58Alphabet, value[i]);
                if (index == -1) {
                    throw new ArgumentException("Illegal character " + value[i] + " at " + i);
                }
                checked {
                    result += multiplier * index;
                    if (i != 0) {
                        multiplier *= FlickrBase58AlphabetLength;
                    }
                }
            }

            return result;
        }

        #region Membres internes.

        internal static long Decode(string value, char[] alphabet, int alphabetLength)
        {
            long result = 0;
            long multiplier  = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.BinarySearch(alphabet, value[i]);
                if (index == -1) {
                    throw new ArgumentException("Illegal character " + value[i] + " at " + i);
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
            string result = String.Empty;

            while (value > 0) {
                long r = value % alphabetLength;
                result = alphabet[r] + result;
                value /= alphabetLength;
            }

            return result;
        }

        #endregion
    }
}
