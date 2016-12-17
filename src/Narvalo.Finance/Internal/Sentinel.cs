// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;

    using static System.Diagnostics.Contracts.Contract;
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
                Narvalo.Require.Range(code.Length == 3, parameterName,
                    Strings.Sentinel_OutOfRangeCurrencyAlphabeticCode);
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

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Demand.True(IsUpperLetter(code));

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Demand.Range(code.Length == 3);
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

                // A currency code MUST only contain uppercase ASCII letters.
                Narvalo.Expect.True(IsUpperLetter(code));

                // A currency code MUST be composed of exactly 3 letters.
                Narvalo.Expect.Range(code.Length == 3);
            }
        }

        public static partial class Warrant
        {
            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void Length(int length)
            {
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length == length);
            }

            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void Lengths(int length1, int length2)
            {
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length == length1 || Result<string>().Length == length2);
            }

            [DebuggerHidden]
            [ContractAbbreviator]
            [Conditional("CONTRACTS_FULL")]
            [ExcludeFromCodeCoverage(
                Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
            public static void LengthRange(int minLength, int maxLength)
            {
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length >= minLength || Result<string>().Length <= maxLength);
            }
        }
    }
}
