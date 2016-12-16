// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

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
                Narvalo.Require.True(code.Length == 3, parameterName,
                    "The alphabetic code MUST be composed of exactly 3 characters.");
            }
        }
    }
}
