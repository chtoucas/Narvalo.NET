// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Benchmarks.Comparisons
{
    using System;

    public static class Int64EncoderComparison
    {
        const string Alphabet = "123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const int AlphabetLength = 58;

        static readonly char[] AlphabetUnorderedArray = new char[] {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 
            't', 'u', 'v', 'w', 'x', 'y', 'z', 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J',
            'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 
            'U', 'V', 'W', 'X', 'Y', 'Z',
        };

        static readonly char[] AlphabetOrderedArray = new char[] {
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

        #region Private methods

        static long Decode_OrderedArray(string value)
        {
            long result = 0;
            long multi = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.BinarySearch(AlphabetOrderedArray, value[i]);

                if (index < 0) {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= AlphabetLength;
            }

            return result;
        }

        static long Decode_String(string value)
        {
            long result = 0;
            long multi = 1;

            while (value.Length > 0) {
                string digit = value.Substring(value.Length - 1);
                int index = Alphabet.LastIndexOf(digit, StringComparison.Ordinal);

                if (index < 0) {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= AlphabetLength;
                value = value.Substring(0, value.Length - 1);
            }

            return result;
        }

        static long Decode_UnorderedArray(string value)
        {
            long result = 0;
            long multi = 1;

            for (int i = value.Length - 1; i >= 0; i--) {
                int index = Array.IndexOf(AlphabetUnorderedArray, value[i]);

                if (index < 0) {
                    throw new ArgumentException("Invalid string", "value");
                }

                result += multi * index;
                multi *= AlphabetLength;
            }

            return result;
        }

        static string Encode_String(long value)
        {
            if (value == 0) {
                return String.Empty;
            }

            string result = string.Empty;

            while (value >= AlphabetLength) {
                int r = (int)(value % AlphabetLength);
                result = Alphabet[r] + result;
                value /= AlphabetLength;
            }

            int index = (int)value;

            if (index > 0) {
                result = Alphabet[index] + result;
            }

            return result;
        }

        static string Encode_Array(long value)
        {
            string result = string.Empty;

            while (value > 0) {
                long r = value % AlphabetLength;
                result = AlphabetOrderedArray[r] + result;
                value /= AlphabetLength;
            }

            return result;
        }

        #endregion
    }
}
