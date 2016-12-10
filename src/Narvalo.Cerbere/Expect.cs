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
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void State(bool testCondition) => True(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void True(bool testCondition) => Contract.Requires(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNullUnconstrained<T>(T value) => True(value != null);

        /// <summary>
        /// Checks that the specified object parameter is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNullOrWhiteSpace(string value)
        {
            // Do not use String.IsNullOrWhiteSpace(), it does not work with CCCheck.
            True(!String.IsNullOrEmpty(value));
            True(!Enforce.IsWhiteSpace(value));
        }
    }

    // Obsolete methods.
    public static partial class Expect
    {
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Expect.NotNull() or Expect.NotNullUnconstrained() instead.", true)]
        public static void Object<T>(T @this) => NotNullUnconstrained(@this);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Expect.NotNull() or Expect.NotNullUnconstrained() instead.", true)]
        public static void Property<T>(T value) => NotNullUnconstrained(value);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Expect.True() instead.", true)]
        public static void Property(bool testCondition) => True(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Expect.NotNullOrEmpty() instead.", true)]
        public static void PropertyNotEmpty(string value) => NotNullOrEmpty(value);
    }
}
