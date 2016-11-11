// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using static System.Diagnostics.Contracts.Contract;

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
    [DebuggerStepThrough]
    public static class Format
    {
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
        public static string CurrentCulture(string format, params object[] args)
        {
            Demand.NotNull(format);
            Demand.NotNull(args);
            Ensures(Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, format, args);
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
        public static string InvariantCulture(string format, params object[] args)
        {
            Demand.NotNull(format);
            Demand.NotNull(args);
            Ensures(Result<string>() != null);

            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array. Actual formatting is done with
        /// the culture used by the current thread.
        /// </summary>
        /// <remarks>Same as <see cref="Format.CurrentCulture"/>, but makes it clear that the format is coming
        /// from a localized resource.</remarks>
        /// <param name="localizedResource">A composite format string obtained from a localized resource.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Resource(string localizedResource, params object[] args)
        {
            Demand.NotNull(localizedResource);
            Demand.NotNull(args);
            Ensures(Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, localizedResource, args);
        }
    }
}
