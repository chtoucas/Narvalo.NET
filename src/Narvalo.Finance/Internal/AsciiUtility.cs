// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    // TODO: Argument validation.
    internal static class AsciiUtility
    {
        public static bool IsDigitOrUpperLetter(string value)
        {
            Promise.NotNull(value, "XXX");

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigitOrUpperLetter(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsDigit(string value)
        {
            Promise.NotNull(value, "XXX");

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigit(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsUpperLetter(string value)
        {
            Promise.NotNull(value, "XXX");

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsUpperLetter(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsDigit(Char c) => c >= '0' && c <= '9';

        private static bool IsDigitOrUpperLetter(Char c) => IsDigit(c) || IsUpperLetter(c);

        private static bool IsUpperLetter(Char c) => c >= 'A' && c <= 'Z';
    }
}
