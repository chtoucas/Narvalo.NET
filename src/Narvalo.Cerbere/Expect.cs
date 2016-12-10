// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides Code Contracts abbreviators.
    /// </summary>
    public static class Expect
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
        public static void NotNullOrWhiteSpace(string value) => True(!String.IsNullOrWhiteSpace(value));

        /// <summary>
        /// Checks that the specified object parameter is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The object to check.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Object<T>(T @this) where T : class => True(@this != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void ObjectNotNull<T>(T @this) => True(@this != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Property(bool testCondition) => True(testCondition);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Property<T>(T value) where T : class => True(value != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void PropertyNotNull<T>(T value) => True(value != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        [Obsolete("Use Expect.PropertyNotNullOrEmpty() instead.")]
        public static void PropertyNotEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void PropertyNotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void PropertyNotNullOrWhiteSpace(string value) => True(!String.IsNullOrWhiteSpace(value));
    }
}
