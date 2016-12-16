// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Diagnostics.Contracts;

    public static partial class AsciiHelpers
    {
        [Pure]
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

        private static bool IsUpperLetter(Char ch) => ch >= 'A' && ch <= 'Z';
    }
}
