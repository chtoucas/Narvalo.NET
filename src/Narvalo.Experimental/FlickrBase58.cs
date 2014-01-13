namespace Narvalo
{
    using System;
    using Narvalo.Diagnostics;

    /// <summary>
    /// Base58 inspired by Flickr's service of url shortening.
    /// Base58 is what you get after taking Base62 [a-zA-Z0-9] and removing any character
    /// that may induce to error when introduced by hand: 0 (zero), O (uppercase 'o'), 
    /// I (uppercase 'i'), and l (lowercase 'L'). This concept was introduced to the general 
    /// public by Flickr, which uses the following String:
    /// </summary>
    public static class FlickrBase58
    {
        const int AlphabetLength_ = 58;
        const long MaxLength_ = 11;

        static readonly char[] Alphabet_ = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
            'W', 'X', 'Y', 'Z',
        };

        public static string Encode(long value)
        {
            Requires.GreaterThanOrEqualTo(value, 0, "value");

            string result = String.Empty;

            while (value > 0) {
                long r = value % AlphabetLength_;
                result = Alphabet_[r] + result;
                value /= AlphabetLength_;
            }

            return result;
        }

        public static long Decode(string value)
        {
            Requires.NotNull(value, "value");
            Requires.LessThanOrEqualTo(value.Length, MaxLength_, "value");

            long result = 0;
            long multiplier = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.IndexOf(Alphabet_, value[i]);
                if (index == -1) {
                    throw new ArgumentException("Illegal character " + value[i] + " at " + i);
                }
                checked {
                    result += multiplier * index;
                    if (i != 0) {
                        multiplier *= AlphabetLength_;
                    }
                }
            }

            return result;
        }
    }
}
