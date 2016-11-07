// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    internal static class ContractFor
    {
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        public static void CurrencyCode(string code)
        {
            // I would like to validate individual characters, but I can't make it work.
            // NB: For PCL classes, we must convert the string to an array before
            // being able to use LINQ operators on it.
            // Contract.Requires(
            //    Contract.ForAll(code.ToCharArray(), c => c >= 65 && c <= 90),
            //    "The code MUST be all CAPS and ASCII.");
            Promise.NotNull(code);
            Promise.Condition(code.Length == 3, "A currency code MUST be composed of exactly 3 letters.");
        }
    }
}
