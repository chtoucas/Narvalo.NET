// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public static class IbanFormat
    {
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        internal const int CountryLength = 2;
        internal const int CheckDigitLength = 2;

        internal const int BbanMinLength = MinLength - CountryLength - CheckDigitLength;
        internal const int BbanMaxLength = MaxLength - CountryLength - CheckDigitLength;

        [Pure]
        public static bool CheckBban(string value)
        {
            if (value == null) { return false; }

            return value.Length >= BbanMinLength && value.Length <= BbanMaxLength;
        }

        [Pure]
        public static bool CheckCheckDigit(string value)
        {
            if (value == null) { return false; }

            return value.Length == CheckDigitLength;
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }

            return value.Length == CountryLength;
        }

        [Pure]
        public static bool CheckInnerValue(string value)
        {
            if (value == null) { return false; }

            return value.Length >= MinLength && value.Length <= MaxLength;
        }
    }
}
