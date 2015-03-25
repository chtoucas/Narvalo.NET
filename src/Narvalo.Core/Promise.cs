// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to check for preconditions that you know to ALWAYS hold.
    /// This is achieved through unrecoverable exceptions in Debug builds.
    /// If Code Contracts are enabled the methods are recognized as preconditions.
    /// Otherwise, the methods are simply discarded by the compiler.
    /// </summary>
    /// <remarks>
    /// This class MUST NOT be used in place of proper validation routines of public 
    /// arguments but rather be reserved for internal sanity checking.
    /// Be wise. Personally, I can only see two situations where these helpers make sense:
    /// for private methods where you have full control of all possible callers and,
    /// only exceptionally, for a few internal methods when you have achieved complete 
    /// Code Contracts coverage.
    /// On purpose, the methods are NOT recognized as parameter validators 
    /// by the Code Analysis tool.
    /// </remarks>
    /// <seealso cref="Check"/>
    [DebuggerStepThrough]
    public static class Promise
    {
        /// <summary>
        /// Asserts a condition.
        /// </summary>
        /// <remarks>
        /// <para>All methods called within the condition must be pure.</para>
        /// <para>All members mentioned in the condition must be at least as visible as the method 
        /// in which they appear.</para>
        /// </remarks>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Condition(bool testCondition, string rationale)
        {
            Contract.Requires(testCondition);

            if (!testCondition)
            {
                throw new FailedPromiseException(
                    Format.CurrentCulture("A promise did not hold: {0}", rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull<T>(T value, string rationale)
        {
            Contract.Requires(value != null);

            if (value == null)
            {
                throw new FailedPromiseException(
                    Format.CurrentCulture("The parameter value is null: {0}", rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrEmpty(string value, string rationale)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));

            if (String.IsNullOrEmpty(value))
            {
                throw new FailedPromiseException(
                    Format.CurrentCulture("The parameter value is null or empty: {0}", rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrWhiteSpace(string value, string rationale)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new FailedPromiseException(
                    Format.CurrentCulture(
                        "The parameter value is null or empty, or consists only of white-space characters: {0}",
                        rationale));
            }
        }
    }
}
