// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    public static class IbanFormat
    {
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        internal const int CountryLength = 2;
        internal const int CheckDigitsLength = 2;

        internal const int BbanMinLength = MinLength - CountryLength - CheckDigitsLength;
        internal const int BbanMaxLength = MaxLength - CountryLength - CheckDigitsLength;

        internal static string RemoveUnwantedCharacters(string value, IbanStyles styles)
        {
            Demand.NotNull(value);
            Warrant.NotNull<string>();

            if (styles.Contains(IbanStyles.None)) { return value; }

            var inarr = value.ToCharArray();
            var outarr = new char[inarr.Length];
            var outlen = 0;

            var removeWhiteSpaces = styles.Contains(IbanStyles.AllowWhiteSpaces);
            var removeDashes = styles.Contains(IbanStyles.AllowDashes);

            for (var i = 0; i < inarr.Length; i++)
            {
                var ch = inarr[i];

                if (removeWhiteSpaces && ch == ' ') { continue; }
                if (removeDashes && ch == '-') { continue; }

                outarr[outlen] = ch;
                outlen++;
            }

            return new String(outarr, 0, outlen);
        }

        [Pure]
        public static bool CheckBban(string value)
        {
            if (value == null) { return false; }
            return value.Length >= BbanMinLength && value.Length <= BbanMaxLength;
        }

        [Pure]
        public static bool CheckCheckDigits(string value)
        {
            if (value == null) { return false; }
            return value.Length == CheckDigitsLength;
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryLength;
        }

        [Pure]
        public static bool CheckValue(string value)
        {
            if (value == null) { return false; }
            return value.Length >= MinLength && value.Length <= MaxLength;
        }
    }
}
