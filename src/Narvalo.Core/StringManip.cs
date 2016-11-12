// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using static System.Diagnostics.Contracts.Contract;

    public static class StringManip
    {
        /// <summary>
        /// Reverses a string.
        /// </summary>
        /// <param name="value">The string to reverse.</param>
        /// <returns>The reversed string.</returns>
        /// <remarks>
        /// This method does work for strings containing surrogate pairs or combining character sequences.
        /// </remarks>
        public static string Reverse(string value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<string>() != null);

            if (value.Length == 0)
            {
                return String.Empty;
            }

            char[] arr = value.ToCharArray();
            Array.Reverse(arr);

            return new String(arr);
        }

        public static string StripCrLf(string value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<string>() != null);

            if (value.Length == 0)
            {
                return String.Empty;
            }

            // TODO: There is clearly a more performant way of doing this.
            return value.Replace("\n", String.Empty).Replace("\r", String.Empty);
        }

        public static string Substring(string value, int startIndex, int length)
        {
            Demand.NotNull(value);
            Demand.True(startIndex >= 0);
            Demand.True(length >= 1);
            Ensures(Result<string>() != null);

            return Substring(value, startIndex, length, "...");
        }

        public static string Substring(string value, int startIndex, int length, string postfix)
        {
            Require.NotNull(value, nameof(value));
            Require.Range(startIndex >= 0, nameof(startIndex));
            Require.Range(length >= 1, nameof(length));
            Ensures(Result<string>() != null);

            if (value.Length <= length)
            {
                // The input string is too short.
                return value;
            }
            else
            {
                if (value.Length < startIndex || value.Length < startIndex + length)
                {
                    // The start index ot the end index is too large.
                    return value.Substring(value.Length - length, length - 1) + postfix;
                }
                else
                {
                    return value.Substring(startIndex, length - 1) + postfix;
                }
            }
        }

        public static string Truncate(string value, int length)
        {
            Demand.NotNull(value);
            Demand.True(length >= 1);
            Ensures(Result<string>() != null);

            return Truncate(value, length, "..." /* postfix */);
        }

        public static string Truncate(string value, int length, string postfix)
        {
            Require.NotNull(value, nameof(value));
            Require.Range(length >= 1, nameof(length));
            Ensures(Result<string>() != null);

            if (value.Length <= length)
            {
                return value;
            }
            else
            {
                return value.Substring(0, length - 1) + postfix;
            }
        }
    }
}
