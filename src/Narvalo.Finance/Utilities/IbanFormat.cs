// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System.Diagnostics.Contracts;

    public static class IbanFormat
    {
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        internal const int CountryLength = 2;
        internal const int CheckDigitsLength = 2;

        internal const int BbanMinLength = MinLength - CountryLength - CheckDigitsLength;
        internal const int BbanMaxLength = MaxLength - CountryLength - CheckDigitsLength;

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
