// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Comparisons
{
    using System;
    using System.Text;

    using Narvalo.Diagnostics.Benchmarking;

    [BenchmarkComparison(10000)]
    public class ReverseComparison
    {
        ////IEnumerable<string> GenerateTestData()
        ////{
        ////    int[] lengths = new int[] {
        ////        5, 50, 100, 150, 200, 250, 300, 350, 400,
        ////        500, 1000, 2000, 
        ////        //3000, 10000,
        ////    };

        ////    return from l in lengths
        ////           select StringGenerator.RandomUnicodeString(l);
        ////}

        //// See also http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        //// See http://weblogs.asp.net/justin_rogers/archive/2004/06/10/153175.aspx
        //// 1. string.ToCharArray - Note, this function is equivalent to net char[] + an internal memory copy.  Most users have this as the basis of their algorithm and I'll show you why this in turn hurts perf.  In effect you are copying the data twice when you don't need to. 
        //// 2. new char[] -This is probably the best buffer you could use.  It has no data in it yet, but strings are immutable and you'll have the original string around the entire time as you do the reversal. 
        //// 3. StringBuilder - StringBuilder's actually use a String behind the scenes and have the ability to convert your *work* into an instantly accessible string.  The char[] overloads for string actually have to copy your buffer into place.  This could be a solution for solving a buffer and reintegration?
        //// The surprising part is how poorly StringBuilder actually performs.  Let's stop and think about what we are doing in the other algorithms.  We are making a copy, doing work on the buffer, then effectively making another copy to turn it back into a string.  The turning it back into a string part is actually a big operation and there isn't anything we can do to limit it's overhead.  Now with a StringBuilder we are making a copy that already is a now mutable string.  We can then update our buffer, and finally get a string back without incurring the extra copy.  To bad all of the bogus thread checking in there kills our performance.
        //// Alrighty, I said three parts.  Turning the buffer back into a string is as easy as constructing a new string, or calling ToString in the case of a StringBuilder.  All there is to it, algorithms are complete.  Here they are, refactored into testable C# with the appropriate attributions to each submitter.

        // Cf. http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        [BenchmarkComparative]
        public static string CharArrayXor(string value)
        {
            char[] arr = value.ToCharArray();
            int len = value.Length - 1;

            for (int i = 0; i < len; i++, len--)
            {
                arr[i] ^= arr[len];
                arr[len] ^= arr[i];
                arr[i] ^= arr[len];
            }

            return new String(arr);
        }

        /// <summary>
        /// This is an inline swap using the ToCharArray method. It involves making a swap 
        /// of two array locations. This means you are going to have a single helper 
        /// variable come along for the ride, since you can't make a swap without storing
        /// a temporary local.  Because of this temporary local, these algorithms fall short a bit.
        /// Note that as the front approaches the back, i &lt; j, there is a point where i = j.  
        /// Most authors actually did a replacement when this occured. 
        /// However, if i = j, then you don't need to do a replacement, and you perform in one
        /// less operation.  Any odd character in the middle of a string is already reversed ;-) 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string CharArrayCopy(string value)
        {
            char[] reversed = value.ToCharArray();

            for (int i = 0, j = reversed.Length - 1; i < j; i++, j--)
            {
                reversed[j] = value[i];
                reversed[i] = value[j];
            }

            return new String(reversed);
        }

        /// <summary>
        /// This is a modified version that uses the original string.
        /// This actually comes out to be a bit faster, because we get rid of one copy for
        /// every 2 characters.  None of the author's found this method, and I can't blame them.
        /// Why in the hell would you use the original string when you have a copy of it ;-) 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string CharArrayCopyFast(string value)
        {
            char[] reversed = value.ToCharArray();

            for (int i = 0, j = reversed.Length - 1; i < j; i++, j--)
            {
                char temp = reversed[i];
                reversed[i] = reversed[j];
                reversed[j] = temp;
            }

            return new String(reversed);
        }

        /// <summary>
        /// This is a surprising algorithm. Because we get back into unmanaged code for the reversal, 
        /// it actually performs better over very large strings than any of the other algorithms.
        /// We are still making the extra copy, but in this case it doesn't matter at all.
        /// Darren found this one all on his own. I wouldn't have even tried it, because logically
        /// finding that ToCharArray was making an extra copy, I would have assumed it to still
        /// be slower (and it still is for strings less than ~100 characters). 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string CharArrayCopyAndReverse(string value)
        {
            char[] arr = value.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// This method uses a character array only. This method gets by doing the original
        /// copy and instead saves all copying for the reversal layer. In this scenario,
        /// you have to iterate until i &lt; j, because you need to restore that middle character.
        /// This generally results in one extra unneeded replacement per run of the algorithm,
        /// but that isn't bad. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string CharArrayInline(string value)
        {
            char[] reversed = new char[value.Length];

            for (int i = 0, j = reversed.Length - 1; i <= j; i++, j--)
            {
                reversed[i] = value[j];
                reversed[j] = value[i];
            }

            return new String(reversed);
        }

        [BenchmarkComparative]
        public static string CharArrayInlineWithSurrogates(string value)
        {
            char[] reversed = new char[value.Length];

            for (int i = 0, j = value.Length - 1; i < value.Length; i++, j--)
            {
                if (value[j] >= 0xD800 && value[j] <= 0xDFFF)
                {
                    reversed[i + 1] = value[j--];
                    reversed[i++] = value[j];
                }
                else
                {
                    reversed[i] = value[j];
                }
            }

            return new String(reversed);
        }

        /// <summary>
        /// Slow, don't use it.  StringBuilder is riddled with thread checks making it extremely slow.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string StringBuilderInline(string value)
        {
            var sb = new StringBuilder(value);

            for (int i = 0, j = value.Length - 1; i <= j; i++, j--)
            {
                sb[i] = value[j];
                sb[j] = value[i];
            }

            return sb.ToString();
        }

        /// <summary>
        /// However, what is a long string? 
        /// Strings less than about 150 characters I'll group into class 1. Strings of about 5000 characters or less I'm going to group into class 2. Anything above is class 3. 
        /// A string-builder matched with buffered chunking using class 1 strings gets it's butt kicked by our other algorithms. In fact, anything less than our buffer size, and it doesn't even help if we use string builders, since the allocated buffer kills any chance of using *less* memory. On the upper end of class 2 strngs the builder method starts to get better again, though it is still getting it's butt kicked. As we start into our class 3 strings, the string builder can actually match the other algorithms because of memory limits and it's reliance on a small work buffer. I used a string of 1024*50 characters in order to demonstrate a class 3 string. I like the results, and I think the string builder might be an adequate scenario for reversing strings when your input data is larger than your work buffer. I do propose a better construct for doing this operation than what I proposed before. This one adds a single division step to get rid of the comparison check each time through. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [BenchmarkComparative]
        public static string StringBuilderCopy(string value)
        {
            var sb = new StringBuilder(value.Length);
            char[] buffer = new char[Math.Min(value.Length, 1024)];

            int fullBuffers = value.Length / 1024;
            int i = value.Length - 1;
            int offset = 0;

            for (int fullBuffer = 0; fullBuffer < fullBuffers; fullBuffer++)
            {
                for (offset = 0; offset < buffer.Length; offset++)
                {
                    buffer[offset] = value[i--];
                }

                sb.Append(buffer, 0, buffer.Length);
            }

            for (offset = 0; i >= 0; i--, offset++)
            {
                buffer[offset] = value[i];
            }

            sb.Append(buffer, 0, offset);

            return sb.ToString();
        }

        ////[BenchComparative]
        ////public unsafe string Unsafe(string value) {
        ////    string output = String.Copy(value);
        ////    fixed (char* pStr = output) {
        ////        char* pStart = pStr;
        ////        char* pEnd = pStr + (output.Length - 1);
        ////        char  temp;
        ////        for (; pStart < pEnd; pStart++, pEnd--) {
        ////            temp = *pStart;
        ////            *pStart = *pEnd;
        ////            *pEnd = temp;
        ////        }
        ////    }
        ////    return output;
        ////}
    }
}
