// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerStepThrough]
    public static partial class Demand
    {
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void State(bool testCondition) => True(testCondition);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void True(bool testCondition)
        {
            Contract.Requires(testCondition);
            Debug.Assert(testCondition);
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullUnconstrained<T>(T value) => True(value != null);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));
    }
}
