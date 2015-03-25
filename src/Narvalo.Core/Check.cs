// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to check for conditions on parameters that you know to ALWAYS hold.
    /// This is achieved through debugging assertions.
    /// </summary>
    /// <remarks>The methods are recognized as parameter validators by the Code Analysis tool.</remarks>
    /// <seealso cref="Promise"/>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>([ValidatedNotNull]T value, string rationale) where T : class
        {
            Debug.Assert(value != null, rationale);
        }

        /// <summary>
        /// Checks that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string rationale)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(value), rationale);
        }
    }
}
