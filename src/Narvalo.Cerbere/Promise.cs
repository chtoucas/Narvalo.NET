// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to state promises and to validate them right away.
    /// </summary>
    /// <remarks>
    /// <para>The methods WON'T be recognized by FxCop as parameter validators
    /// against <see langword="null"/> value.</para>
    /// <para>The methods will be recognized as Contract Abbreviator methods.</para>
    /// <para>If a promise does not hold, an unrecoverable exception is thrown
    /// in debug builds.</para>
    /// <para>This class MUST NOT be used in place of proper validation routines of public
    /// arguments but rather be reserved for internal sanity checking. Be wise.
    /// Personally, I can only see three situations where these helpers make sense:
    /// <list type="bullet">
    /// <item>for private methods where you have full control of all possible callers.</item>
    /// <item>for protected overridden methods in a sealed class when the base method does
    /// not declare any contract, when you know for certain that *ALL* callers will
    /// satisfy the condition and most certainly when you own all base classes.
    /// As you can see, that makes a lot of prerequisites...</item>
    /// <item>and exceptionally for a few internal methods when you have achieved complete
    /// Code Contracts coverage.</item>
    /// </list>
    /// </para>
    /// </remarks>
    [DebuggerStepThrough]
    public static class Promise
    {
        //[ContractAbbreviator]
        //[Conditional("DEBUG")]
        //[Conditional("CONTRACTS_FULL")]
        //public static void True(bool testCondition)
        //{
        //    Contract.Requires(testCondition);
        //    Debug.Assert(testCondition);
        //}

        /// <summary>
        /// Validates that a condition holds.
        /// </summary>
        /// <remarks>
        /// <para>All methods called within the condition must be pure.</para>
        /// <para>All members mentioned in the condition must be at least as visible as the method
        /// in which they appear.</para>
        /// </remarks>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void True(bool testCondition, string rationale)
        {
            Contract.Requires(testCondition);

            if (!testCondition)
            {
                throw new IllegalConditionException("A promise did not hold: " + rationale);
            }
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Condition(bool testCondition, string rationale)
        {
            Contract.Requires(testCondition);

            if (!testCondition)
            {
                throw new IllegalConditionException("A promise did not hold: " + rationale);
            }
        }

        //[ContractAbbreviator]
        //[Conditional("DEBUG")]
        //[Conditional("CONTRACTS_FULL")]
        //public static void NotNull<T>(T value) where T : class
        //{
        //    Contract.Requires(value != null);
        //    Debug.Assert(value != null);
        //}

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull<T>(T value, string rationale) where T : class
        {
            Contract.Requires(value != null);

            if (value == null)
            {
                throw new IllegalConditionException("The parameter value is null: " + rationale);
            }
        }

        //[ContractAbbreviator]
        //[Conditional("DEBUG")]
        //[Conditional("CONTRACTS_FULL")]
        //public static void NotNullOrEmpty(string value)
        //{
        //    Contract.Requires(!String.IsNullOrEmpty(value));
        //    Debug.Assert(!String.IsNullOrEmpty(value));
        //}

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrEmpty(string value, string rationale)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));

            if (String.IsNullOrEmpty(value))
            {
                throw new IllegalConditionException("The parameter value is null or empty: " + rationale);
            }
        }

        //[ContractAbbreviator]
        //[Conditional("DEBUG")]
        //[Conditional("CONTRACTS_FULL")]
        //public static void NotNullOrWhiteSpace(string value)
        //{
        //    Contract.Requires(!String.IsNullOrWhiteSpace(value));
        //    Debug.Assert(!String.IsNullOrWhiteSpace(value));
        //}

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrWhiteSpace(string value, string rationale)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new IllegalConditionException(
                    "The parameter value is null or empty, or consists only of white-space characters: " + rationale);
            }
        }
    }
}
