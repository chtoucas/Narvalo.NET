// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods to check for preconditions that you know to ALWAYS hold.
    /// This is achieved through both debugging assertions and Code Contracts preconditions.
    /// </summary>
    /// <remarks>
    /// <para>These helpers MUST NOT be used in place of proper validation routines of public 
    /// arguments but they should rather be reserved for internal sanity checking.</para>
    /// <para>If Code Contracts are enabled the methods are recognized as preconditions; otherwise
    /// - In Debug builds, the methods turn into debugging assertions.
    /// - Otherwise, the methods are simply discarded by the compiler.</para>
    /// </remarks>
    /// <seealso cref="Assume"/>.
    [DebuggerStepThrough]
    internal static class Promise
    {
        /// <summary>
        /// Checks for a condition.
        /// </summary>
        /// <remarks>
        /// <para>All methods called within the condition must be pure.</para>
        /// <para>All members mentioned in the condition must be at least as visible as the method 
        /// in which they appear.</para>
        /// </remarks>
        /// <param name="condition">The conditional expression to evaluate.</param>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [DebuggerHidden]
        [ContractAbbreviator]
        public static void Condition(bool condition)
        {
            Contract.Requires(condition);

            Debug.Assert(condition, "A promise did not hold.");
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [DebuggerHidden]
        [ContractAbbreviator]
        public static void NotNull<T>(T value) where T : class
        {
            Contract.Requires(value != null);

            Debug.Assert(value != null, "The parameter is null.");
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [DebuggerHidden]
        [ContractAbbreviator]
        public static void NotNullOrEmpty(string value)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));

            Debug.Assert(!String.IsNullOrEmpty(value), "The parameter is null or empty.");
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [DebuggerHidden]
        [ContractAbbreviator]
        public static void NotNullOrWhiteSpace(string value)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));

            Debug.Assert(
                !String.IsNullOrWhiteSpace(value),
                "The parameter is null or empty, or consists only of white-space characters");
        }
    }
}
