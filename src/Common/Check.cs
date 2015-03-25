// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Provides helper methods to check for promises on parameters.
    /// </summary>
    /// <remarks>
    /// <para>The methods will be recognized as parameter validators by FxCop.</para>
    /// <para>The methods MUST appear after all Code Contracts.</para>
    /// <para>If a promise does not hold, a message is sent to the debugging listeners.</para>
    /// <para>This class MUST NOT be used in place of proper validation routines of public
    /// arguments but is only useful in very specialized use cases. Be wise.
    /// Personally, I can only see three situations where these helpers make sense:
    /// for protected overriden methods in a sealed class when the base method does
    /// have a contract attached AND when you know for certain that all callers will
    /// satisfy the condition.
    /// </para>
    /// </remarks>
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>([ValidatedNotNull]T value) where T : class
        {
            Debug.Assert(value != null, "The parameter value is null.");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>([ValidatedNotNull]T value, string rationale) where T : class
        {
            Debug.Assert(value != null, rationale);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value)
        {
            Debug.Assert(!String.IsNullOrEmpty(value), "The parameter value is null or empty.");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string rationale)
        {
            Debug.Assert(!String.IsNullOrEmpty(value), rationale);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value)
        {
            Debug.Assert(
                !String.IsNullOrWhiteSpace(value),
                "The parameter value is null or empty, or consists only of white-space characters");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="rationale">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string rationale)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(value), rationale);
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
        private sealed class ValidatedNotNullAttribute : Attribute { }
    }
}
