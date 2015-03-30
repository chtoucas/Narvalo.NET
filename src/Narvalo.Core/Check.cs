// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides helper methods to check for promises on parameters.
    /// </summary>
    /// <remarks>
    /// <para>The methods WON'T be recognized as parameter validators by FxCop.</para>
    /// <para>The methods MUST appear after all Code Contracts.</para>
    /// <para>If a condition does not hold, a message is sent to the debugging listeners
    /// and an unrecoverable exception is thrown.</para>
    /// <para>This class MUST NOT be used in place of proper validation routines of public
    /// arguments but is only useful in very specialized use cases. Be wise.
    /// Personally, I can only see one situation where these helpers make sense:
    /// for protected overriden methods in a sealed class when the base method
    /// declares a contract (otherwise you should use Narvalo.Promise instead)
    /// AND when you know for certain that all callers will satisfy the condition.
    /// </para>
    /// </remarks>
    public static class Check
    {
        /// <summary>
        /// Checks that a condition holds.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="rationale">The rationale for the promise.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Condition(bool testCondition, string rationale)
        {
            if (!testCondition)
            {
                throw new FailedCheckException("A condition did not hold: " + rationale);
            }
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value, string rationale) where T : class
        {
            if (value == null)
            {
                throw new FailedCheckException("The parameter value is null: " + rationale);
            }
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value, string rationale)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new FailedCheckException("The parameter value is null or empty: " + rationale);
            }
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace(string value, string rationale)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new FailedCheckException(
                    "The parameter value is null or empty, or consists only of white-space characters: " + rationale);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
            Justification = "[Intentionally] This is an unrecoverable exception, thrown when a supposedly impossible situation happened.")]
        [SuppressMessage("Gendarme.Rules.Exceptions", "MissingExceptionConstructorsRule",
            Justification = "[Intentionally] This exception can not be initialized outside this assembly.")]
        private sealed class FailedCheckException : Exception
        {
            public FailedCheckException(string message) : base(message) { }
        }
    }
}
