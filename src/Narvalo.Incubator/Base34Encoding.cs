namespace Narvalo {
    using System;
    using System.Diagnostics.Contracts;

    public static class Base34Encoding {
        private const int AlphabetLength = 34;

        private const long MaxLength = 13;

        //private const long MaxMultiplier = Int64.MaxValue / AlphabetLength;

        // Ordered alphabet.
        private static readonly char[] Alphabet = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
            't', 'u', 'v', 'w', 'x', 'y', 'z', 
        };

        public static long Decode(string value) {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentOutOfRangeException>(value.Length <= MaxLength);

            long result = 0;
            long multiplier  = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.BinarySearch(Alphabet, value[i]);
                if (index == -1) {
                    throw new ArgumentException("Illegal character " + value[i] + " at " + i);
                }

                //// TODO
                //if (result > Int64.MaxValue - multiplier * index) {
                //    throw new ArgumentException("Illegal sequence of chars");
                //}
                result += multiplier * index;

                if (i != 0) {
                    //// TODO
                    //if (multiplier > MaxMultiplier) {
                    //    throw new ArgumentException("Illegal sequence of chars");
                    //}
                    multiplier *= AlphabetLength;
                }
            }

            return result;
        }

        public static string Encode(long value) {
            Contract.Requires<ArgumentOutOfRangeException>(value >= 0);

            string result = string.Empty;

            while (value > 0) {
                long r = value % AlphabetLength;
                result = Alphabet[r] + result;
                value /= AlphabetLength;
            }

            return result;
        }
    }
}
