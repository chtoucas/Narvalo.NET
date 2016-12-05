// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Internal.AsciiHelpers;

    internal static class ContractFor
    {
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void CurrencyCode(string code)
        {
            Expect.NotNull(code);

            // A currency code MUST only contain uppercase ASCII letters.
            Expect.True(IsUpperLetter(code));

            // A currency code MUST be composed of exactly 3 letters.
            Expect.True(code.Length == 3);
        }
    }
}
