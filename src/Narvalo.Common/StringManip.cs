// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;

    public static class StringManip
    {
        // Cf. http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        public static string RemoveDiacritics(string value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length == 0)
            {
                return String.Empty;
            }

            var formD = value.Normalize(NormalizationForm.FormD).AssumeNotNull();

            var sb = new StringBuilder();

            for (int i = 0; i < formD.Length; i++)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(formD[i]) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(formD[i]);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).AssumeNotNull();
        }

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
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length == 0) {
                return String.Empty;
            }

            char[] arr = value.ToCharArray();
            Array.Reverse(arr);

            return new String(arr);
        }

        [SuppressMessage("Gendarme.Rules.Portability", "NewLineLiteralRule",
            Justification = "[Intentionally] This method does not depend on platform specific rules.")]
        public static string StripCrLf(string value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length == 0) {
                return String.Empty;
            }

            // TODO: There is clearly a more performant way of doing this.
            return value.Replace("\n", String.Empty).Replace("\r", String.Empty);
        }

        public static string Substring(string value, int startIndex, int length)
        {
            Contract.Requires(value != null);
            Contract.Requires(startIndex >= 0);
            Contract.Requires(length >= 1);
            Contract.Ensures(Contract.Result<string>() != null);

            return Substring(value, startIndex, length, "...");
        }

        public static string Substring(string value, int startIndex, int length, string postfix)
        {
            Require.NotNull(value, "value");
            Require.GreaterThanOrEqualTo(startIndex, 0, "startIndex");
            Require.GreaterThanOrEqualTo(length, 1, "length");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length <= length) {
                // The input string is too short.
                return value;
            }
            else {
                if (value.Length < startIndex || value.Length < startIndex + length) {
                    // The start index ot the end index is too large.
                    return Format.CurrentCulture("{0}{1}", value.Substring(value.Length - length, length - 1), postfix);
                }
                else {
                    return Format.CurrentCulture("{0}{1}", value.Substring(startIndex, length - 1), postfix);
                }
            }
        }

        public static string Truncate(string value, int length)
        {
            Contract.Requires(value != null);
            Contract.Requires(length >= 1);
            Contract.Ensures(Contract.Result<string>() != null);

            return Truncate(value, length, postfix: "...");
        }

        public static string Truncate(string value, int length, string postfix)
        {
            Require.NotNull(value, "value");
            Require.GreaterThanOrEqualTo(length, 1, "length");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length <= length) {
                return value;
            }
            else {
                return Format.CurrentCulture("{0}{1}", value.Substring(0, length - 1), postfix);
            }
        }

        public static string ToTitleCase(string value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            if (value.Length == 0) {
                return value;
            }
            else {
                // REVIEW: First value.ToLowerInvariant()?
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            }
        }
    }
}
