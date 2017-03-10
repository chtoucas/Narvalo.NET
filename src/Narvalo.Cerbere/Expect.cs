// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides Code Contracts abbreviators.
    /// </summary>
    public static partial class Expect
    {
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void State(bool testCondition) => True(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void True(bool testCondition) => Contract.Requires(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void NotNullUnconstrained<T>(T value) => True(value != null);

        /// <summary>
        /// Checks that the specified object parameter is not null or empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));
    }
}
