// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    internal static class AsciiHelpers
    {
        public static bool IsDigitOrUpperLetter(string value)
        {
            Promise.NotNull(value, "Prove that any call to this internal method is guarded upstream.");

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
            Promise.NotNull(value, "Prove that any call to this internal method is guarded upstream.");

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
            Promise.NotNull(value, "Prove that any call to this internal method is guarded upstream.");

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsUpperLetter(value[i]))
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
