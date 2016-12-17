// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
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

        internal static string RemoveDisplayCharacters(string value, IbanStyles styles)
        {
            Demand.NotNull(value);
            Warrant.NotNull<string>();

            var inarr = value.ToCharArray();
            var outarr = new char[inarr.Length];
            var outlen = 0;

            for (var i = 0; i < inarr.Length; i++)
            {
                var ch = inarr[i];

                if (styles.Contains(IbanStyles.AllowWhiteSpaces) && ch == ' ')
                {
                    continue;
                }
                if (styles.Contains(IbanStyles.AllowDashes) && ch == '-')
                {
                    continue;
                }

                outarr[outlen] = ch;
                outlen++;
            }

            return new String(outarr, 0, outlen);
        }

        internal static bool Validate(Iban iban)
        {
            throw new NotImplementedException();
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
