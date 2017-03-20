// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides helper methods to format strings with the culture used by the
    /// current thread. Only meant to be used when the composite format string
    /// is culture-aware.
    /// </summary>
    [DebuggerStepThrough]
    public static class Format
    {
        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="arg"/>.</typeparam>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">The object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format item or items
        /// have been replaced by the string representation of <paramref name="arg"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T>(string format, T arg)
            => String.Format(CultureInfo.CurrentCulture, format, arg);

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of two specified objects. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <typeparam name="T0">The type of <paramref name="arg0"/>.</typeparam>
        /// <typeparam name="T1">The type of <paramref name="arg1"/>.</typeparam>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the items
        /// have been replaced by the string representation of <paramref name="arg0"/>
        /// and <paramref name="arg1"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1>(string format, T0 arg0, T1 arg1)
            => String.Format(CultureInfo.CurrentCulture, format, arg0, arg1);

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of three specified objects. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <typeparam name="T0">The type of <paramref name="arg0"/>.</typeparam>
        /// <typeparam name="T1">The type of <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="T2">The type of <paramref name="arg2"/>.</typeparam>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>A copy of <paramref name="format"/> in which the items
        /// have been replaced by the string representation of <paramref name="arg0"/>,
        /// <paramref name="arg1"/> and <paramref name="arg2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
            => String.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);
    }
}
