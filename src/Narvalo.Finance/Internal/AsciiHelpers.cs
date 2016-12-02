// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;
    using System.Diagnostics.Contracts;

    // REVIEW: Believe this is wrong.
#if CONTRACTS_FULL
    public
#else
    internal
#endif
        static class AsciiHelpers
    {
        [Pure]
        public static bool IsDigitOrUpperLetter(string value)
        {
            // Prove that any call to this internal method is guarded upstream.
            Demand.NotNull(value);

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigitOrUpperLetter(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        [Pure]
        public static bool IsDigit(string value)
        {
            // Prove that any call to this internal method is guarded upstream.
            Demand.NotNull(value);

            for (int i = 0; i < value.Length; i++)
            {
                if (!IsDigit(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        [Pure]
        public static bool IsUpperLetter(string value)
        {
            // Prove that any call to this internal method is guarded upstream.
            Demand.NotNull(value);

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
