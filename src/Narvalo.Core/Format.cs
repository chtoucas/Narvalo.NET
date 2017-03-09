// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
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
    [Pure]
    [DebuggerStepThrough]
    public static class Format
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current(string format)
            => String.Format(CultureInfo.CurrentCulture, format);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0>(string format, T0 arg0)
            => String.Format(CultureInfo.CurrentCulture, format, arg0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1>(string format, T0 arg0, T1 arg1)
            => String.Format(CultureInfo.CurrentCulture, format, arg0, arg1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
            => String.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Current(string format, params object[] args)
            => String.Format(CultureInfo.CurrentCulture, format, args);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant(string format)
            => String.Format(CultureInfo.InvariantCulture, format);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0>(string format, T0 arg0)
            => String.Format(CultureInfo.InvariantCulture, format, arg0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0, T1>(string format, T0 arg0, T1 arg1)
            => String.Format(CultureInfo.InvariantCulture, format, arg0, arg1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
            => String.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is culture-independent.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invariant(string format, params object[] args)
            => String.Format(CultureInfo.InvariantCulture, format, args);
    }
}
