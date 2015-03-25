// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// Checks for a condition.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Condition(bool testCondition)
        {
            Debug.Assert(testCondition, "A condition did not hold.");
        }

        /// <summary>
        /// Checks for a condition.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="message">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Condition(bool testCondition, string message)
        {
            Debug.Assert(testCondition, message);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value) where T : class
        {
            Debug.Assert(value != null, "The value is null.");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="message">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value, string message) where T : class
        {
            Debug.Assert(value != null, message);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            Debug.Assert(!String.IsNullOrEmpty(value), "The value is null or empty.");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="message">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value, string message)
        {
            Debug.Assert(!String.IsNullOrEmpty(value), message);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace(string value)
        {
            Debug.Assert(
                !String.IsNullOrWhiteSpace(value),
                "The value is null or empty, or consists only of white-space characters");
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="message">The message to send to display if the test fails.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullOrWhiteSpace(string value, string message)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(value), message);
        }
    }
}
