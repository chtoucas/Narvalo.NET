// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    [DebuggerStepThrough]
    public static class Format
    {
        [Pure]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CurrentCulture(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        [Pure]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CurrentUICulture(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.CurrentUICulture, format, args);
        }

        [Pure]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string InvariantCulture(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        [Pure]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Resource(string localizedResource, params object[] args)
        {
            Contract.Requires(localizedResource != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return String.Format(CultureInfo.CurrentCulture, localizedResource, args);
        }
    }
}
