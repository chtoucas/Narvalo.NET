// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.AsciiHelpers;

    internal static class Guards
    {
        public static class Demand
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void CurrencyCode(string code)
            {
                Narvalo.Demand.NotNull(code);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Demand.True(IsUpperLetter(code));

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Demand.True(code.Length == 3);
            }
        }

        public static class Expect
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void CurrencyCode(string code)
            {
                Narvalo.Expect.NotNull(code);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Expect.True(IsUpperLetter(code));

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Expect.True(code.Length == 3);
            }
        }

        public static class Warrant
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void Length(int length)
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length == length);
            }
        }
    }
}
