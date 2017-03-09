// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    [Pure]
    internal static partial class Ascii
    {
        public static bool IsUpperLetter(string value)
        {
            if (value == null || value.Length == 0) { return false; }

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsUpperLetter(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsDigit(string value)
        {
            if (value == null || value.Length == 0) { return false; }

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigit(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsDigitOrUpperLetter(string value)
        {
            if (value == null || value.Length == 0) { return false; }

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigitOrUpperLetter(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsDigit(Char ch) => ch >= '0' && ch <= '9';

        private static bool IsDigitOrUpperLetter(Char ch) => IsDigit(ch) || IsUpperLetter(ch);

        private static bool IsUpperLetter(Char ch) => ch >= 'A' && ch <= 'Z';
    }
}
