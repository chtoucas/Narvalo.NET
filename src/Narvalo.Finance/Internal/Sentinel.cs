// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;

    internal static partial class Sentinel
    {
        public static partial class Require
        {
            [ContractArgumentValidator]
            public static void CurrencyCode(string code, string parameterName)
            {
                Narvalo.Require.NotNull(code, parameterName);

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Require.Range(code.Length == 3, parameterName,
                    Strings.Sentinel_OutOfRangeCurrencyAlphabeticCode);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Require.True(Ascii.IsUpperLetter(code), parameterName);
            }
        }

        public static partial class Demand
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void CurrencyCode(string code)
            {
                Narvalo.Demand.NotNull(code);

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Demand.Range(code.Length == 3);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Demand.True(Ascii.IsUpperLetter(code));
            }
        }

        public static partial class Expect
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void CurrencyCode(string code)
            {
                Narvalo.Expect.NotNull(code);

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Expect.Range(code.Length == 3);

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Expect.True(Ascii.IsUpperLetter(code));
            }
        }
    }
}
