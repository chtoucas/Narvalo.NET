// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Comparisons
{
    using System;

    public static class Int64EncoderComparison
    {
        private const string ALPHABET = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        private const int ALPHABET_LENGTH = 58;

        private static readonly char[] s_AlphabetUnorderedArray = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
            't', 'u', 'v', 'w', 'x', 'y', 'z', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J',
            'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 
            'U', 'V', 'W', 'X', 'Y', 'Z',
        };

        private static readonly char[] s_AlphabetOrderedArray = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J',
            'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 
            'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
            't', 'u', 'v', 'w', 'x', 'y', 'z', 
        };

        ////public static void ConvertInt64_Decode() {
        ////    int howMany = 20000;
        ////    var alts = ConvertInt64Tests.Decode();
        ////    var test = new SpeedTest<string>(alts);
        ////    var results =  test.Time(howMany, "6hJsyS");

        ////    DisplayResult("ConvertInt64.Decode()", howMany, results);
        ////}

        ////public static void ConvertInt64_Encode() {
        ////    int howMany = 20000;
        ////    var alts = ConvertInt64Tests.Encode();
        ////    var test = new SpeedTest<long>(alts);
        ////    var results = test.Time(howMany, 3471131850);

        ////    DisplayResult("ConvertInt64.Encode()", howMany, results);
        ////}

        ////public static IDictionary<string, long> Decode(int iterations, string value)
        ////{
        ////    var list = new BenchmarkCollection<string>();

        ////    // The ordered array should be the fastest.
        ////    list.Add("Ordered array", (p) => Decode_OrderedArray(p));
        ////    list.Add("String", (p) => Decode_String(p));
        ////    list.Add("Unordered array", (p) => Decode_UnorderedArray(p));

        ////    var test = BenchComparer.Create(new WallTimer(), list);

        ////    return test.Compare(iterations, value /* "6hJsyS" */);
        ////}

        ////public static IDictionary<string, long> Encode(int iterations, long value)
        ////{
        ////    var list = new BenchmarkCollection<long>();

        ////    // Both should give similar results.
        ////    list.Add("Array", (p) => Encode_Array(p));
        ////    list.Add("String", (p) => Encode_String(p));

        ////    var test = BenchComparer.Create(new WallTimer(), list);

        ////    return test.Compare(iterations, value /* 3471131850 */);
        ////}

        private static long Decode_OrderedArray(string value)
        {
            long result = 0;
            long multi = 1;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                int index = Array.BinarySearch(s_AlphabetOrderedArray, value[i]);

                if (index < 0)
                {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= ALPHABET_LENGTH;
            }

            return result;
        }

        private static long Decode_String(string value)
        {
            long result = 0;
            long multi = 1;

            while (value.Length > 0)
            {
                string digit = value.Substring(value.Length - 1);
                int index = ALPHABET.LastIndexOf(digit, StringComparison.Ordinal);

                if (index < 0)
                {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= ALPHABET_LENGTH;
                value = value.Substring(0, value.Length - 1);
            }

            return result;
        }

        private static long Decode_UnorderedArray(string value)
        {
            long result = 0;
            long multi = 1;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                int index = Array.IndexOf(s_AlphabetUnorderedArray, value[i]);

                if (index < 0)
                {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= ALPHABET_LENGTH;
            }

            return result;
        }

        private static string Encode_String(long value)
        {
            if (value == 0)
            {
                return String.Empty;
            }

            string result = String.Empty;

            while (value >= ALPHABET_LENGTH)
            {
                int r = (int)(value % ALPHABET_LENGTH);
                result = ALPHABET[r] + result;
                value /= ALPHABET_LENGTH;
            }

            int index = (int)value;

            if (index > 0)
            {
                result = ALPHABET[index] + result;
            }

            return result;
        }

        private static string Encode_Array(long value)
        {
            string result = String.Empty;

            while (value > 0)
            {
                long r = value % ALPHABET_LENGTH;
                result = s_AlphabetOrderedArray[r] + result;
                value /= ALPHABET_LENGTH;
            }

            return result;
        }
    }
}
