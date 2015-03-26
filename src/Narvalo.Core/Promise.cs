// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods to state promises and to validate them right away.
    /// </summary>
    /// <remarks>
    /// <para>The methods WON'T be recognized as parameter validators by FxCop.</para>
    /// <para>The methods will be recognized as Contract Abbreviator methods.</para>
    /// <para>If a promise does not hold, an unrecoverable exception is thrown.</para>
    /// <para>This class MUST NOT be used in place of proper validation routines of public 
    /// arguments but rather be reserved for internal sanity checking. Be wise.
    /// Personally, I can only see three situations where these helpers make sense:
    /// <list type="bullet">
    /// <item>for private methods where you have full control of all possible callers.</item>
    /// <item>for protected overriden methods in a sealed class when the base method does
    /// not have any contract attached AND when you know for certain that all callers will 
    /// satisfy the condition.</item>
    /// <item>and exceptionally for a few internal methods when you have achieved complete
    /// Code Contracts coverage.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public static class Promise
    {
        /// <summary>
        /// Promises and checks that a condition holds.
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
        /// Promises and checks that the specified argument is not <see langword="null"/>.
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
        /// Promises and checks that the specified argument is not <see langword="null"/> or empty.
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
        /// Promises and checks that the specified argument is not <see langword="null"/> or empty,
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

        [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
            Justification = "[Intentionally] This is an unrecoverable exception, thrown when a supposedly impossible situation happened.")]
        [SuppressMessage("Gendarme.Rules.Exceptions", "MissingExceptionConstructorsRule",
            Justification = "[Intentionally] This exception can not be initialized outside this assembly.")]
        private sealed class FailedPromiseException : Exception
        {
            public FailedPromiseException(string message) : base(message) { }
        }
    }
}
