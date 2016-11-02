// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    internal static class CharsUtility
    {
        public static bool IsDigit(Char c) => IsDigit((int)c);

        public static bool IsDigit(int pos) => pos >= 48 && pos <= 57;

        public static bool IsUpperLetter(Char c) => IsUpperLetter((int)c);

        public static bool IsUpperLetter(int pos) => pos >= 65 && pos <= 90;
    }
}
