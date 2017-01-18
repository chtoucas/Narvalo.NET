// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides helper methods to format strings.
    /// </summary>
    /// <remarks>
    /// <para>The methods are nothing but aliases for <see cref="String.Format(IFormatProvider, string, object[])"/>
    /// but they clearly state which culture is used for formatting.</para>
    /// <para>To avoid any performance penalty we explicitly ask the runtime to inline them;
    /// I am pretty sure this is not necessary but it should be safe to do so.</para>
    /// <para>Do not create an alias for <see cref="CultureInfo.CurrentUICulture"/>
    /// since it has nothing to do with formatting.</para>
    /// </remarks>
    // REVIEW: https://github.com/dotnet/corefx/issues/1514
    [DebuggerStepThrough]
    public static class Format
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current(string format)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.CurrentCulture, format);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0>(string format, T0 arg0)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.CurrentCulture, format, arg0);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.CurrentCulture, format, arg0, arg1);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current(string format, params object[] args)
        {
            Expect.NotNull(format);
            Expect.NotNull(args);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant(string format)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.InvariantCulture, format);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0>(string format, T0 arg0)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.InvariantCulture, format, arg0);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.InvariantCulture, format, arg0, arg1);
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            Expect.NotNull(format);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is culture-independent.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant(string format, params object[] args)
        {
            Expect.NotNull(format);
            Expect.NotNull(args);
            Warrant.NotNull<string>();

            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
