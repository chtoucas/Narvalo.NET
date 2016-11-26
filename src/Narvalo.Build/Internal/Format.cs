// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.Internal
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides helper methods to format strings.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Format
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
        public static string Current(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
