// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.LocalData.Snv
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.LocalData.Properties;

    internal static partial class Sentinel
    {
        public static partial class Require
        {
            [ContractAbbreviator]
            public static void CurrencyCode(string code, string parameterName)
            {
                Narvalo.Require.NotNull(code, parameterName);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Require.True(IsUpperLetter(code), parameterName);

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Require.Range(code.Length == 3, parameterName,
                    Strings.Sentinel_OutOfRangeCurrencyAlphabeticCode);
            }
        }

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
