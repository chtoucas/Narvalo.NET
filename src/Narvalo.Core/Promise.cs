// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to check for preconditions that you know to ALWAYS hold.
    /// This is achieved through unrecoverable exceptions in DEBUG builds 
    /// and Code Contracts preconditions when their enabled.
    /// </summary>
    /// <remarks>
    /// <para>These helpers MUST NOT be used in place of proper validation routines of public 
    /// arguments but they should rather be reserved for internal sanity checking.</para>
    /// <para>If Code Contracts are enabled the methods are recognized as preconditions; otherwise
    /// - In Debug builds, the methods turn into debugging assertions.
    /// - Otherwise, the methods are simply discarded by the compiler.</para>
    /// </remarks>
    [DebuggerStepThrough]
    public static class Promise
    {
        /// <summary>
        /// Checks for a condition.
        /// </summary>
        /// <remarks>
        /// <para>All methods called within the condition must be pure.</para>
        /// <para>All members mentioned in the condition must be at least as visible as the method 
        /// in which they appear.</para>
        /// </remarks>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        /// <param name="file">Optional full path of the source file that contains the caller.</param>
        /// <param name="line">Optional line number in the source file at which the method is called.</param>
        /// <param name="memberName">Optional method or property name of the caller to the method.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "[Intentionally] The parameters are filled automatically by the compler.")]
        public static void Condition(
            bool testCondition,
            string rationale,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = null)
        {
            Contract.Requires(testCondition);

            if (!testCondition)
            {
                throw new FailedPromiseException(
                    Format.InvariantCulture(
                        "{0}:{1} [{2}] - A promise did not hold: {3}",
                        file,
                        line,
                        memberName,
                        rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        /// <param name="file">Optional full path of the source file that contains the caller.</param>
        /// <param name="line">Optional line number in the source file at which the method is called.</param>
        /// <param name="memberName">Optional method or property name of the caller to the method.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "[Intentionally] The parameters are filled automatically by the compler.")]
        public static void NotNull<T>(
            T value,
            string rationale,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = null)
        {
            Contract.Requires(value != null);

            if (value == null)
            {
                throw new FailedPromiseException(
                    Format.InvariantCulture(
                        "{0}:{1} [{2}] - The parameter is null: {3}",
                        file,
                        line,
                        memberName,
                        rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        /// <param name="file">Optional full path of the source file that contains the caller.</param>
        /// <param name="line">Optional line number in the source file at which the method is called.</param>
        /// <param name="memberName">Optional method or property name of the caller to the method.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "[Intentionally] The parameters are filled automatically by the compler.")]
        public static void NotNullOrEmpty(
            string value,
            string rationale,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = null)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));

            if (String.IsNullOrEmpty(value))
            {
                throw new FailedPromiseException(
                    Format.InvariantCulture(
                        "{0}:{1} [{2}] - The parameter is null or empty: {3}",
                        file,
                        line,
                        memberName,
                        rationale));
            }
        }

        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        /// <param name="file">Optional full path of the source file that contains the caller.</param>
        /// <param name="line">Optional line number in the source file at which the method is called.</param>
        /// <param name="memberName">Optional method or property name of the caller to the method.</param>
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "[Intentionally] The parameters are filled automatically by the compler.")]
        public static void NotNullOrWhiteSpace(
            string value,
            string rationale,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string memberName = null)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new FailedPromiseException(
                    Format.InvariantCulture(
                        "{0}:{1} [{2}] - The parameter is null or empty, or consists only of white-space characters: {3}",
                        file,
                        line,
                        memberName,
                        rationale));
            }
        }
    }
}
